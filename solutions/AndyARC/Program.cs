using System.Text.Json;
using AndyARC.Core;

var actionsFromSystem2 = new List<Func<int[][], int[][]>>
{
    Knowledge.DeDupeXY
};

// todo: define an ARC-AGI solver with a rhizome structure
Console.WriteLine("ARC-AGI Solver");
Console.WriteLine("Training set...");
var trainingFiles = Directory.GetFiles("../../data/training", "*.json");
SolvePuzzles(trainingFiles, actionsFromSystem2);

// eval
var evalFiles = Directory.GetFiles("../../data/evaluation", "*.json");
Console.WriteLine("Evaluation set...");
SolvePuzzles(evalFiles, actionsFromSystem2);

static void SolvePuzzles(string[] puzzleFiles, List<Func<int[][], int[][]>> actions)
{
    var tests = 0;
    var wins = 0;
    foreach (var filePath in puzzleFiles)
    {
        var puz = JsonSerializer.Deserialize<Puzzle>(File.ReadAllText(filePath))
            ?? throw new Exception($"Failed to load puzzle from {filePath}");
        // training samples
        int[][] modified;
        foreach (var action in actions)
        {
            var taskNumber = 0;
            foreach (var t in puz.Train)
            {
                modified = action(t.Input);

                if (IsMatch(t, modified))
                    Console.WriteLine($"Train WIN: {Path.GetFileName(filePath)} task {taskNumber} - {action.Method.Name}");

                taskNumber++;
            }
            foreach (var t in puz.Test)
            {
                // test samples
                modified = action(t.Input);
                if (IsMatch(t, modified))
                {
                    wins++;
                    Console.WriteLine($"TEST WIN: {Path.GetFileName(filePath)} - {action.Method.Name}");
                }
                tests++;
            }
        }
    }
    Console.WriteLine($"Tests: {tests}, Wins: {wins}");
}

static bool IsMatch(ARCSample t, int[][] solution)
{
    var match = t.Output.Length == solution.Length;
    for (var i = 0; i < t.Output.Length; i++)
    {
        match = match && t.Output[i].SequenceEqual(solution[i]);
    }

    return match;
}