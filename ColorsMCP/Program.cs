using ColorsCommonMCP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ColorsMCP
{
    public class Program
    {
   
        static async Task Main(string[] args)
        {
            var builder = Host.CreateEmptyApplicationBuilder(settings: null);
            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithTools<ColorsTools>();

            builder.Services.AddSingleton<ColorsService>();

            var app = builder.Build();

            var colorsService = app.Services.GetRequiredService<ColorsService>();

            await app.RunAsync();

        }
    }
}
