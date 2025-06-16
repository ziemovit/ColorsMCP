using ColorsCommonMCP;

namespace ColorsMCP_HTTP
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
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

            app.UseCors();

            app.MapMcp();

            app.MapGet("/health", () => "Healthy");

            app.Run();
        }
    }
}
