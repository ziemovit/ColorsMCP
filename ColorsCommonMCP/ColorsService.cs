using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColorsCommonMCP;

public class ColorsService
{
    private readonly List<Colors> colors;

    public ColorsService()
    {
        var assembly = typeof(ColorsService).Assembly;
        using var stream = assembly.GetManifestResourceStream("ColorsCommonMCP.colors.json");
        using var reader = new StreamReader(stream!);
        var json = reader.ReadToEnd();
        colors = JsonSerializer.Deserialize(json, ColorsContext.Default.ListColors) ?? new List<Colors>();
    }

    List<Colors> colorsList = new();
    public async Task<List<Colors>> GetColors()
    {
        await Task.Run(() => { });

        return colors;
    }

    public async Task<List<Colors>> GetColorsByFamily(string family)
    {
        await Task.Run(() => { });

        return colors.Where(c => c.Families != null && c.Families.Contains(family, StringComparer.OrdinalIgnoreCase)).ToList();
    }

    public async Task<Colors?> GetColors(string name)
    {
        await Task.Run(() => { });

        return colors.FirstOrDefault(m => m.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
    }
}

public partial class Colors
{
    public string? Name { get; set; } 
    public string? Hexcode { get; set; }   
    public string? RGB { get; set; }   
    public List<string>? Families { get; set; }  
}

[JsonSerializable(typeof(List<Colors>))]
[JsonSerializable(typeof(Colors))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class ColorsContext : JsonSerializerContext
{
}
