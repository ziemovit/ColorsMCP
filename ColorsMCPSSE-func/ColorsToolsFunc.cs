using ColorsCommon;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorsMCPSSE_func
{
    public class ColorsToolsFunc
    {
        private readonly ILogger<ColorsToolsFunc> _logger;
        private readonly ColorsService _colorsService;

        public ColorsToolsFunc(ILogger<ColorsToolsFunc> logger, ColorsService colorsService)
        {
            _logger = logger;
            _colorsService = colorsService;
        }

        [Function(ColorsInfo.GetAllColorsToolName)]
        public async Task<string> GetAllColors(
            [McpToolTrigger(ColorsInfo.GetAllColorsToolName, ColorsInfo.GetAllColorsToolDescription)]
            ToolInvocationContext context
        )
        {
            var colors = await _colorsService.GetColors();
            return JsonSerializer.Serialize(colors);
        }

        [Function(ColorsInfo.GetColorsByFamilyToolName)]
        public async Task<string> GetColorsByFamily(
            [McpToolTrigger(ColorsInfo.GetColorsByFamilyToolName, ColorsInfo.GetColorsByFamilyToolDescription)]
            ToolInvocationContext context,
                    [McpToolProperty("Name", "string", ColorsInfo.GetColorsByFamilyParamFamilyDescription)]
            string family
        )
        {
            var colors = await _colorsService.GetColorsByFamily(family);
            return JsonSerializer.Serialize(colors);
        }

        [Function(ColorsInfo.GetColorToolName)]
        public async Task<string> GetColor(
            [McpToolTrigger(ColorsInfo.GetColorToolName, ColorsInfo.GetColorToolDescription)]
            ToolInvocationContext context,
            [McpToolProperty("Name", "string", ColorsInfo.GetColorParamNameDescription)]
            string name
    )
        {
            var colors = await _colorsService.GetColors(name);
            return JsonSerializer.Serialize(colors);
        }

    }
}
