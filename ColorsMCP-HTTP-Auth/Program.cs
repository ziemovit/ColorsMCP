using ColorsCommonMCP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModelContextProtocol.AspNetCore.Authentication;
using System.Security.Claims;

namespace ColorsMCP_HTTP
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var serverUrl = builder.Configuration["ServerUrl"] ?? "";
            var tenantId = builder.Configuration["TenantId"] ?? "";
            var audience = builder.Configuration["Audience"] ?? "";
            var scope = builder.Configuration["Scope"] ?? "";
            var oAuthServerUrl = $"https://login.microsoftonline.com/{tenantId}/v2.0";

            builder.Services
                .AddAuthentication(options =>
                    {
                        options.DefaultChallengeScheme = McpAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(options =>
                    {
                        options.Authority = oAuthServerUrl;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidAudience = $"api://{audience}",
                            ValidIssuers = new[]
                            {
                                oAuthServerUrl,
                                $"https://sts.windows.net/{tenantId}/",
                            },
                            NameClaimType = "name",
                            RoleClaimType = "roles"
                        };

                        options.MetadataAddress = $"{oAuthServerUrl}/.well-known/openid-configuration";

                        options.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                var name = context.Principal?.Identity?.Name ?? "unknown";
                                var upn = context.Principal?.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn") ?? "unknown";
                                //foreach (var claim in context.Principal?.Claims ?? Enumerable.Empty<Claim>())
                                //{
                                //    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                                //}
                                //Console.ForegroundColor = ConsoleColor.Cyan;
                                //Console.WriteLine($"JWT Token: {context.SecurityToken}");
                                //Console.ResetColor();
 
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Token validated for: {name} ({upn})");
                                Console.ResetColor();
                                return Task.CompletedTask;
                            },
                            OnAuthenticationFailed = context =>
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                                Console.ResetColor();
                                return Task.CompletedTask;
                            }
                        };
                    })
                .AddMcp(options =>
                {
                    options.ResourceMetadata = new()
                    {
                        Resource = new Uri(serverUrl),
                        ResourceDocumentation = new Uri("https://github.com/markharrison/colorsmcp"),
                        AuthorizationServers = { new Uri(oAuthServerUrl) },
                        ScopesSupported = [$"api://{audience}/{scope}"],
                    };

                });

            builder.Services.AddAuthorization();
            //          builder.Services.AddHttpContextAccessor();

            builder.Services
                .AddMcpServer()
                .WithHttpTransport()
                .WithTools<ColorsTools>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddSingleton<ColorsService>();

            var app = builder.Build();

            var colorsService = app.Services.GetRequiredService<ColorsService>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.MapMcp().RequireAuthorization();

            app.MapGet("/health", () => "Healthy");

            Console.WriteLine($"Starting MCP server with authz: {serverUrl}");
            Console.WriteLine($"OAuth server at {oAuthServerUrl}");
            Console.WriteLine($"Protected Resource Metadata URL: {serverUrl}.well-known/oauth-protected-resource");

            app.Run();
        }
    }
}
