using System.Reflection;
using System.Diagnostics;
using WindowsillSoft.CodeChallenges.Core;

var solverAssemblies = new[]
{
    typeof(WindowsillSoft.CodeChallenges.AdventOfCode.AdventOfCodeSolverBase).Assembly,
    typeof(WindowsillSoft.CodeChallenges.ProjectEuler.ProjectEulerSolverBase).Assembly
};

// Find all solver types
var solverTypes = solverAssemblies
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsClass && !t.IsAbstract && typeof(ProblemSolverBase).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToList();

Console.WriteLine($"Found {solverTypes.Count} solvers\n");

if (args.Length == 0)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  list [year]              List all solvers or solvers for a specific year");
    Console.WriteLine("  run <year> <day> [input] Run a specific solver");
    Console.WriteLine("  test <year> <day>        Run solver with example input");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  list 2019");
    Console.WriteLine("  run 2019 1");
    Console.WriteLine("  test 2019 1");
    return;
}

string command = args[0].ToLower();

switch (command)
{
    case "list":
        {
            int? year = args.Length > 1 ? int.Parse(args[1]) : null;
            
            foreach (var type in solverTypes)
            {
                var category = type.GetCustomAttribute<SolverCategoryAttribute>();
                if (category != null)
                {
                    if (year.HasValue && !category.Category.Contains(year.Value.ToString()))
                        continue;
                    
                    Console.WriteLine($"{category.Category,-20} {type.Name}");
                }
            }
            break;
        }
        
    case "run":
    case "test":
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Error: Specify year and day");
                return;
            }
            
            int year = int.Parse(args[1]);
            int day = int.Parse(args[2]);
            string? customInput = args.Length > 3 ? args[3] : null;
            
            // Find the solver
            var solverType = solverTypes.FirstOrDefault(t =>
            {
                var category = t.GetCustomAttribute<SolverCategoryAttribute>();
                return category != null && 
                       category.Category.Contains(year.ToString()) &&
                       t.Name.Contains($"Day{day:D2}");
            });
            
            if (solverType == null)
            {
                Console.WriteLine($"No solver found for {year} Day {day}");
                return;
            }
            
            // Create IOProvider
            var provider = new ConsoleIOProvider(customInput, command == "test");
            var solver = Activator.CreateInstance(solverType, provider) as ProblemSolverBase;
            
            if (solver == null)
            {
                Console.WriteLine("Failed to create solver instance");
                return;
            }
            
            try
            {
                Console.WriteLine($"Running {solver.Name}\n");
                
                var sw = Stopwatch.StartNew();
                solver.Initialize();
                var initTime = sw.Elapsed;
                
                sw.Restart();
                var result = solver.Execute();
                var execTime = sw.Elapsed;
                
                Console.WriteLine($"\nResults:");
                Console.WriteLine($"  {result}");
                Console.WriteLine($"\nTiming:");
                Console.WriteLine($"  Initialize: {initTime.TotalMilliseconds:F2}ms");
                Console.WriteLine($"  Execute:    {execTime.TotalMilliseconds:F2}ms");
                Console.WriteLine($"  Total:      {(initTime + execTime).TotalMilliseconds:F2}ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            
            break;
        }
        
    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}

// Simple console-based IOProvider
class ConsoleIOProvider(string? customInput, bool isTest) : IIOProvider
{
    private readonly string? _customInput = customInput;
    private readonly bool _isTest = isTest;

    public string RequestFile(string filePath)
    {
        // If custom input provided, use it
        if (_customInput != null)
            return _customInput;
        
        // Check for example input file if in test mode
        if (_isTest)
        {
            var examplePath = filePath.Replace(".txt", "_example.txt");
            if (File.Exists(examplePath))
            {
                Console.WriteLine($"Loading example input from: {examplePath}");
                return File.ReadAllText(examplePath);
            }
        }
        
        // Load from file
        if (File.Exists(filePath))
        {
            Console.WriteLine($"Loading input from: {filePath}");
            return File.ReadAllText(filePath);
        }
        
        // Fallback: prompt user
        Console.WriteLine($"File not found: {filePath}");
        Console.WriteLine("Enter input (Ctrl+Z or Ctrl+D when done):");
        
        var lines = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            lines.Add(line);
        }
        
        return string.Join('\n', lines);
    }

    public int RequestChoice(string prompt, params string[] choices)
    {
        Console.WriteLine(prompt);
        for (int i = 0; i < choices.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {choices[i]}");
        }
        
        Console.Write("Choice: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= choices.Length)
        {
            return choice - 1;
        }
        
        return 0;
    }
}
