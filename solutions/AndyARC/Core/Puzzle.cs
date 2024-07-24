using System.Text.Json.Serialization;

namespace AndyARC.Core;

public class Puzzle(IEnumerable<ARCSample> train, IEnumerable<ARCSample> test)
{
    [JsonPropertyName("train")]
    public IEnumerable<ARCSample> Train { get; } = train;
    [JsonPropertyName("test")]
    public IEnumerable<ARCSample> Test { get; } = test;
}