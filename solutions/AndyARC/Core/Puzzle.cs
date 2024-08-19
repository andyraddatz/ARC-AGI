using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndyARC.Core;

public class Puzzle(IEnumerable<ARCSample> train, IEnumerable<ARCSample> test)
{
    public static Puzzle FromJson(string filePath)
    {
        var puz = JsonSerializer.Deserialize<Puzzle>(File.ReadAllText(filePath))
            ?? throw new ApplicationException($"Failed to load puzzle from {filePath}");
        puz.Name = Path.GetFileNameWithoutExtension(filePath);
        return puz;
    }

    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("train")]
    public IEnumerable<ARCSample> Train { get; } = train;
    [JsonPropertyName("test")]
    public IEnumerable<ARCSample> Test { get; } = test;
}