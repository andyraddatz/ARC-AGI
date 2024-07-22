using System.Text.Json;
using AndyARC.Core;

// todo: define an ARC-AGI solver with a rhizome structure
var files = Directory.GetFiles("../../data/training", "*.json");
var wins = 0;
foreach (var filePath in files)
{
    var puz = JsonSerializer.Deserialize<Puzzle>(File.ReadAllText(filePath))
        ?? throw new Exception("Failed to deserialize puzzle");

    // training samples
    foreach (var t in puz.Train)
    {
        var deduped = Knowledge.DeDupeXY(t.Input);
        var match = t.Output.Length == deduped.Length;
        for (var i = 0; i < t.Output.Length; i++)
        {
            match = match && t.Output[i].SequenceEqual(deduped[i]);
        }

        if (match)
            Console.WriteLine($"WIN: {Path.GetFileName(filePath)}");
    }

    // test samples
    foreach (var t in puz.Test)
    {
        var deduped = Knowledge.DeDupeXY(t.Input);
        if (puz.TrySolveTest([new(t.Input, deduped)]))
        {
            wins++;
            Console.WriteLine($"TEST WIN: {Path.GetFileName(filePath)}");
        }
    }
}

Console.WriteLine($"Wins: {wins}");