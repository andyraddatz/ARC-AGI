using System.Text.Json.Serialization;
using AndyARC.Core.Features;

namespace AndyARC.Core;

public class ARCSample(int[][] input, int[][] output)
{
    [JsonPropertyName("input")]
    public int[][] Input { get; set; } = input;
    [JsonPropertyName("output")]
    public int[][] Output { get; set; } = output;

    // objectness
    public IEnumerable<IEnumerable<(int, int)>> InputObjects { get; } = SystemOne.DetectObjects(input);
    public IEnumerable<IEnumerable<(int, int)>> OutputObjects { get; } = SystemOne.DetectObjects(output);

    // goal-directedness
    public IEnumerable<GoalFeature>? GoalFeatures { get; set; } = null;
}