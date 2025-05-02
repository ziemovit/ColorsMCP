using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace ColorsCommon;

[McpServerToolType]
public sealed class ColorsTools
{
    private readonly ColorsService colorsService;

    public ColorsTools(ColorsService colorsService)
    {
        this.colorsService = colorsService;
    }

    [McpServerTool, Description("Get a list of all colors.")]
    public async Task<string> GetAllColors()
    {
        var colors = await colorsService.GetColors();
        return JsonSerializer.Serialize(colors, ColorsContext.Default.ListColors);
    }

    [McpServerTool, Description("Get a list of colors by family group.")]
    public async Task<string> GetColorByFamily([Description("Get a list of colors that belong to a family group")] string family)
    {
        var colors = await colorsService.GetColorsByFamily(family);
        return JsonSerializer.Serialize(colors, ColorsContext.Default.ListColors);
    }


    [McpServerTool, Description("Get a colors by name.")]
    public async Task<string> GetColor([Description("The name of the color to get details for")] string name)
    {
        var colors = await colorsService.GetColors(name);
        return JsonSerializer.Serialize(colors, ColorsContext.Default.Colors);
    }
}
