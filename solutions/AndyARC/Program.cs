using System.Text.Json;
using AndyARC.Core;

Console.ForegroundColor = ConsoleColor.White;

// ARC-AGI solver
Console.WriteLine("ARC-AGI Solver");

// training
Console.WriteLine("Training set...");
var trainingFiles = Directory.GetFiles("../../data/training", "*.json");
var (tests, wins) = SystemTwo.SolvePuzzles(trainingFiles);
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"Training set Tests: {tests}, Wins: {wins}");

// eval
var evalFiles = Directory.GetFiles("../../data/evaluation", "*.json");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Evaluation set...");
(tests, wins) = SystemTwo.SolvePuzzles(evalFiles);
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"Evaluation set Tests: {tests}, Wins: {wins}");