using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ColorsCommon;

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
