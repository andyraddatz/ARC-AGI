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
    public IEnumerable<ObjectFeature> InputObjects { get; } = SystemOne.ExtractObjects(input);
    public IEnumerable<ObjectFeature>? OutputObjects { get; } = SystemOne.ExtractObjects(output);

    // goal-directedness
    public IEnumerable<GoalFeature>? GoalFeatures { get; set; } = null;

}