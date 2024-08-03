using System.Text.Json;

namespace AndyARC.Core;

public static class SystemTwo
{
    public static (int, int) SolvePuzzles(string[] puzzleFilePaths)
    {
        Console.ForegroundColor = default;
        // todo: lots more actions
        var puzzles = 0;
        var wins = 0;
        foreach (var filePath in puzzleFilePaths)
        {
            var puz = JsonSerializer.Deserialize<Puzzle>(File.ReadAllText(filePath))
                ?? throw new Exception($"Failed to load puzzle from {filePath}");

            var action = GetAction(puz, Path.GetFileName(filePath));
            var win = false;
            foreach (var t in puz.Test)
            {
                var modified = action(t.Input);
                win = IsMatch(t.Output, modified);
            }
            if (win)
            {
                wins++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"TEST WIN: {Path.GetFileName(filePath)}", ConsoleColor.Green);
                Console.ForegroundColor = default;
            }
            puzzles++;
        }
        return (puzzles, wins);
    }

    private static Func<int[][], int[][]> GetAction(Puzzle puz, string puzName)
    {

        Func<int[][], int[][]> compoundAction = (x) =>
        {
            var modified = x;
            foreach (var action in SystemOne.Actions)
            {
                modified = action(modified);
            }
            return modified;
        };
        var allFuncs = SystemOne.Actions;
        allFuncs = allFuncs.Append(compoundAction);

        foreach (var action in allFuncs)
        {
            // if we find any action that works for all training tasks
            var candidateFound = true;
            foreach (var t in puz.Train)
            {
                var modified = action(t.Input);
                if (IsMatch(t.Output, modified))
                {
                    Console.WriteLine($"Training match: {puzName} - {action.Method.Name}");
                    candidateFound = candidateFound && IsMatch(t.Output, modified);
                }
                candidateFound = candidateFound && IsMatch(t.Output, modified);
            }

            // AND is plausible for the test tasks, we're done
            candidateFound = candidateFound && IsPlausible(action, puz.Test);
            if (candidateFound) return action;
        }

        return _ => _;
    }

    private static bool IsPlausible(Func<int[][], int[][]> action, IEnumerable<ARCSample> test)
    {
        var isPlausible = false;
        foreach (var t in test)
        {
            // todo: define plausibility
            var modified = action(t.Input);
            // output grid is within possible bounds
            isPlausible = modified.Length <= 30 && modified.All(row => row.Length <= 30);
            // input grid has features expected by the action
        }
        return isPlausible;
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