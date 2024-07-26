using System.Text.Json;

namespace AndyARC.Core;

public static class SystemTwo
{
    public static (int, int) SolvePuzzles(string[] puzzleFiles)
    {
        // todo: lots more actions
        var actions = SystemOne.Actions;
        var tests = 0;
        var wins = 0;
        foreach (var filePath in puzzleFiles)
        {
            var puz = JsonSerializer.Deserialize<Puzzle>(File.ReadAllText(filePath))
                ?? throw new Exception($"Failed to load puzzle from {filePath}");

            // training samples
            var taskNumber = 0;
            foreach (var t in puz.Train)
            {
                foreach (var action in actions)
                {
                    var modified = action(t.Input);

                    if (IsMatch(t.Output, modified))
                        Console.WriteLine($"Train WIN: {Path.GetFileName(filePath)} task {taskNumber} - {action.Method.Name}");
                }
                taskNumber++;
            }

            taskNumber = 0;
            foreach (var t in puz.Test)
            {
                foreach (var action in actions)
                {
                    // test samples
                    var modified = action(t.Input);
                    if (IsMatch(t.Output, modified))
                    {
                        wins++;
                        Console.WriteLine($"TEST WIN: {Path.GetFileName(filePath)} task {taskNumber} - {action.Method.Name}");
                    }
                }
                taskNumber++;
                tests++;
            }
        }
        return (tests, wins);
    }

    static bool IsMatch(int[][] actual, int[][] proposed)
    {
        var match = actual.Length == proposed.Length;
        for (var i = 0; i < actual.Length; i++)
        {
            match = match && actual[i].SequenceEqual(proposed[i]);
        }

        return match;
    }
}