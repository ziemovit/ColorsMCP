using ModelContextProtocol.Client;

namespace MCPClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("*** MCP Client ***");

            //var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
            //{
            //    Name = "ColorsMCP",
            //    Command = "dotnet",
            //    Arguments = ["run", "--project", "c:/dev/ColorsMCP/ColorsMCP/ColorsMCP.csproj"]
            //});

            var clientTransport = new SseClientTransport(new SseClientTransportOptions
            {
                Name = "ColorsMCP",
                Endpoint = new Uri("https://colorsmcp.azure1.dev/")
            });


            var client = await McpClientFactory.CreateAsync(clientTransport);

            foreach (var tool in await client.ListToolsAsync())
            {
                Console.WriteLine($"{tool.Name} ({tool.Description})");
            }
            Console.WriteLine($"---");

            var result = await client.CallToolAsync(
                "GetColor",
                new Dictionary<string, object?>() { ["name"] = "Red" },
                cancellationToken: CancellationToken.None);

            foreach (var textBlock in result.Content.Where(c => c.Type == "text"))
            {
                var textProperty = textBlock.GetType().GetProperty("Text");
                if (textProperty != null)
                {
                    Console.WriteLine(textProperty.GetValue(textBlock));
                }
            }

        }
    }
}
