namespace AndyARC.Core;

public class Puzzle(IEnumerable<ARCSample> train, IEnumerable<ARCSample> test)
{
    public IEnumerable<ARCSample> Train { get; } = train;
    public IEnumerable<ARCSample> Test { get; } = test;
    public bool TrySolveTest(IEnumerable<ARCSample> solution)
    {
        // todo: this
        return Test.All(t => solution.Any(s => s.Input == t.Input && s.Output == t.Output));
    }
}

public class ARCSample(int[][] input, int[][] output)
{
    public int[][] Input { get; set; } = input;
    public int[][] Output { get; set; } = output;
}