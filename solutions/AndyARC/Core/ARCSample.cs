using System.Text.Json.Serialization;

namespace AndyARC.Core;

public class ARCSample(int[][] input, int[][] output)
{
    [JsonPropertyName("input")]
    public int[][] Input { get; set; } = input;
    [JsonPropertyName("output")]
    public int[][] Output { get; set; } = output;
}