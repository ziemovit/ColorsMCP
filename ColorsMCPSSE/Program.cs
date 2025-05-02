using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ColorsCommon;

namespace ColorsMCPSSE
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

            builder.Services.AddSingleton<ColorsService>();

            var app = builder.Build();

            var colorsService = app.Services.GetRequiredService<ColorsService>();

            app.MapMcp();

            app.Run();
        }
    }
}
