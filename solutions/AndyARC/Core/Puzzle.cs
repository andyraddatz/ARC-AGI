using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AndyARC.Core;

public class Puzzle(IEnumerable<ARCSample> train, IEnumerable<ARCSample> test)
{
    [JsonPropertyName("train")]
    public IEnumerable<ARCSample> Train { get; } = train;
    [JsonPropertyName("test")]
    public IEnumerable<ARCSample> Test { get; } = test;
    public bool TrySolveTest(IEnumerable<ARCSample> solution)
    {
        var match = true;
        foreach (var t in Test)
        {
            // assume there is a solution for each test
            var s = solution.FirstOrDefault(s => s.Input.SequenceEqual(t.Input));
            if (s == null)
            {
                match = false;
                continue;
            }
            match = match && t.Output.Length == s.Output.Length;
            for (var i = 0; i < t.Output.Length; i++)
            {
                match = match && t.Output[i].SequenceEqual(s.Output[i]);
            }
        }
        return match;
    }
}

public class ARCSample(int[][] input, int[][] output)
{
    [JsonPropertyName("input")]
    public int[][] Input { get; set; } = input;
    [JsonPropertyName("output")]
    public int[][] Output { get; set; } = output;
}