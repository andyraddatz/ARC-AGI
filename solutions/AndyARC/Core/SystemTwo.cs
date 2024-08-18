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
            var puz = Puzzle.FromJson(filePath);
            var action = GetAction(puz);
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
                Console.WriteLine($"TEST WIN: {puz.Name}");
                Console.ForegroundColor = default;
            }
            puzzles++;
        }
        return (puzzles, wins);
    }

    private static Func<int[][], int[][]> GetAction(Puzzle puz)
    {
        // todo: algorithmize problem solving
        foreach (var t in puz.Train)
        {
            // list objects of the training inputs
            // t.InputObjects = SystemOne.ExtractObjects(t.Input);
            // list objects of the training outputs
            // t.OutputObjects = SystemOne.ExtractObjects(t.Output);
            // list features of the goal
            t.GoalFeatures = SystemOne.ExtractGoalFeatures(t.Input, t.Output);
        }

        foreach (var action in GenerateActions(puz))
        {
            // if we find any action that works for all training tasks
            var candidateFound = true;
            foreach (var t in puz.Train)
            {
                var modified = action(t.Input);
                if (IsMatch(t.Output, modified))
                {
                    Console.WriteLine($"Training match: {puz.Name} - {action.Method.Name}");
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

    private static IEnumerable<Func<int[][], int[][]>> GenerateActions(Puzzle puz)
    {
        // list features of the test inputs
        // foreach (var t in puz.Test) t.InputObjects = SystemOne.ExtractObjects(t.Input);

        // devise action plan
        //      find features in common between training inputs + test inputs
        //      generate hypotheses for action(s) that transforms ARC inputs to outputs
        //          verify hypothesis for all training inputs->outputs
        //          verify action is plausible for the test inputs
        // use action plan to generate test outputs
        foreach (var a in SystemOne.Actions) yield return a;
        Func<int[][], int[][]> compoundAction = (x) =>
        {
            var modified = x;
            foreach (var action in SystemOne.Actions)
            {
                modified = action(modified);
            }
            return modified;
        };
        yield return compoundAction;
        // var allFuncs = SystemOne.Actions.Append(compoundAction);
        // return allFuncs;
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