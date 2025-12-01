# CodeChallenges Driver

A unified console driver for running WindowsillSoft.CodeChallenges solvers.

## Features

- **List solvers**: Browse all available challenge solvers by year
- **Run solvers**: Execute specific solvers with custom or file-based input
- **Test mode**: Automatically load example input files for testing
- **Timing**: Measure initialization and execution time
- **Easy input**: Supply input via file, command line, or interactive prompt

## Usage

### List all solvers
```bash
dotnet run -- list
```

### List solvers for a specific year
```bash
dotnet run -- list 2019
```

### Run a solver with its input file
```bash
dotnet run -- run 2019 1
```

### Run a solver with custom input
```bash
dotnet run -- run 2019 1 "12345\n67890"
```

### Test mode with example input
```bash
dotnet run -- test 2019 1
```

Test mode looks for input files with `_example.txt` suffix (e.g., `input_example.txt`) before falling back to the regular input file.

## Example Input Files

To use test mode effectively, organize your input files as:
```
/path/to/inputs/
├── 2019_day01.txt          # Actual puzzle input
├── 2019_day01_example.txt  # Example from puzzle description
```

The driver will automatically find example files when using `test` command.

## Architecture

- Uses reflection to discover all `ProblemSolverBase` implementations
- Provides a simple `ConsoleIOProvider` for file and console I/O
- Supports both AdventOfCode and ProjectEuler solver frameworks
- Reports detailed timing for performance analysis
