using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace ColorsCommonMCP;

[McpServerToolType]
public sealed class ColorsTools
{
    private readonly ColorsService colorsService;

    public ColorsTools(ColorsService colorsService)
    {
        this.colorsService = colorsService;
    }

    [McpServerTool, Description(ColorsInfo.GetAllColorsToolDescription)]
    public async Task<string> GetAllColors()
    {
        var colors = await colorsService.GetColors();
        return JsonSerializer.Serialize(colors, ColorsContext.Default.ListColors);
    }

    [McpServerTool, Description(ColorsInfo.GetColorsByFamilyToolDescription)]    
    public async Task<string> GetColorByFamily([Description(ColorsInfo.GetColorsByFamilyParamFamilyDescription)] string family)
    {
        var colors = await colorsService.GetColorsByFamily(family);
        return JsonSerializer.Serialize(colors, ColorsContext.Default.ListColors);
    }


    [McpServerTool, Description(ColorsInfo.GetColorToolDescription)]
    public async Task<string> GetColor([Description(ColorsInfo.GetColorParamNameDescription)] string name)
    {
        var colors = await colorsService.GetColors(name);
        return JsonSerializer.Serialize(colors, ColorsContext.Default.Colors);
    }
}
