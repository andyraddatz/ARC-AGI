using System.Text.Json;
using AndyARC.Core;

// ARC-AGI solver
Console.WriteLine("ARC-AGI Solver");

// training
Console.WriteLine("Training set...");
var trainingFiles = Directory.GetFiles("../../data/training", "*.json");
var (tests, wins) = SystemTwo.SolvePuzzles(trainingFiles);
Console.WriteLine($"Training set Tests: {tests}, Wins: {wins}");

// eval
var evalFiles = Directory.GetFiles("../../data/evaluation", "*.json");
Console.WriteLine("Evaluation set...");
(tests, wins) = SystemTwo.SolvePuzzles(evalFiles);
Console.WriteLine($"Evaluation set Tests: {tests}, Wins: {wins}");