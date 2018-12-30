using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day20
{
    public class Day20Solver : AoC2018SolverBase
    {
        private Dictionary<(int x, int y), Direction> _maze;

        public override string Description => "Day 20: A Regular Map";

        public override int SortOrder => 20;

        public override void Initialize(string input)
        {
            _maze = new Dictionary<(int x, int y), Direction>();
            var location = (x: 0, y: 0);
            _maze[location] = Direction.None;

            var BranchPoints = new Stack<(int x, int y)>();
            foreach (var command in input)
            {
                switch (command)
                {
                    case '^': /*Check for index = 0?*/ break;
                    case '$': /*Check that no other commands are enqueued?*/ break;
                    case '(': BranchPoints.Push(location); break;
                    case '|': location = BranchPoints.Peek(); break;
                    case ')': location = BranchPoints.Pop(); break;
                    default: location = HandleNavigationDirection(_maze, location, command); break;
                }
            }
        }

        public override string SolvePart1(bool silent = true)
        {
            if (!silent)
            {
                Console.CursorVisible = false;

                Console.Clear();
                WriteMaze(_maze);
            }

            var (distances, finalLocation) = SolveMaze(silent);
            if (!silent)
            {
                Console.CursorTop = _maze.Max(p => p.Key.y) + 1;
                Console.WriteLine($"Explortation stopped at {finalLocation} with distance {distances[finalLocation.x, finalLocation.y] - 1}.");
                Console.CursorVisible = true;
            }

            return (distances[finalLocation.x, finalLocation.y] - 1).ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            var (distances, finalLocation) = SolveMaze(silent);

            var longDistances = (
                from x in Enumerable.Range(0, distances.GetLength(0))
                from y in Enumerable.Range(0, distances.GetLength(1))
                select (x, y))
                .Count(p => distances[p.x, p.y] > 1000);

            return $"{longDistances}";
        }

        private (int[,] Distances, (int x, int y) FinalLocation) SolveMaze(bool silent)
        {
            var fringe = new List<(int x, int y)> { (0, 0) };
            var offset = (x: _maze.Min(p => p.Key.x), y: _maze.Min(p => p.Key.y));
            var distances = new int[_maze.Max(p => p.Key.x) - offset.x + 1, _maze.Max(p => p.Key.y) - offset.y + 1];
            distances[-offset.x, -offset.y] = 1;
            (int x, int y) location = default;
            while (fringe.Any())
            {
                var probe = fringe.First();
                location = (probe.x - offset.x, probe.y - offset.y);

                fringe.RemoveAt(0);
                var room = _maze[probe];
                if (room.HasFlag(Direction.North) && distances[location.x, location.y - 1] == default)
                {
                    distances[location.x, location.y - 1] = distances[location.x, location.y] + 1;
                    fringe.Add((probe.x, probe.y - 1));
                }
                if (room.HasFlag(Direction.South) && distances[location.x, location.y + 1] == default)
                {
                    distances[location.x, location.y + 1] = distances[location.x, location.y] + 1;
                    fringe.Add((probe.x, probe.y + 1));
                }
                if (room.HasFlag(Direction.East) && distances[location.x + 1, location.y] == default)
                {
                    distances[location.x + 1, location.y] = distances[location.x, location.y] + 1;
                    fringe.Add((probe.x + 1, probe.y));
                }
                if (room.HasFlag(Direction.West) && distances[location.x - 1, location.y] == default)
                {
                    distances[location.x - 1, location.y] = distances[location.x, location.y] + 1;
                    fringe.Add((probe.x - 1, probe.y));
                }

                if (!silent)
                {
                    (Console.CursorLeft, Console.CursorTop) = (location.x, location.y);
                    Console.WriteLine(distances[location.x, location.y] % 10);
                }
            }
            return (distances, location);
        }

        private void WriteMaze(Dictionary<(int x, int y), Direction> maze)
        {
            Dictionary<Direction, char> mapping = new Dictionary<Direction, char>
            {
                { Direction.None, 'X' },
                { Direction.North, 'O' },
                { Direction.South, 'O' },
                { Direction.East, 'O' },
                { Direction.West, 'O' },
                { Direction.North | Direction.South, '│' },
                { Direction.North | Direction.East, '└'},
                { Direction.North | Direction.West, '┘' },
                { Direction.North | Direction.South | Direction.East, '├' },
                { Direction.North | Direction.South | Direction.West, '┤' },
                { Direction.North | Direction.East | Direction.West, '┴' },
                { Direction.North | Direction.South | Direction.East | Direction.West, '┼' },
                { Direction.South | Direction.East,'┌' },
                { Direction.South | Direction.West, '┐' },
                { Direction.South | Direction.East | Direction.West, '┬' },
                { Direction.East | Direction.West, '─' },
            };
            var minX = maze.Keys.Min(p => p.x);
            var minY = maze.Keys.Min(p => p.y);
            foreach (var room in maze)
            {
                (Console.CursorLeft, Console.CursorTop) = (room.Key.x - minX, room.Key.y - minY);
                Console.Write(mapping[room.Value]);
            }

            Console.ReadKey();
        }

        private (int x, int y) HandleNavigationDirection(Dictionary<(int, int), Direction> maze, (int x, int y) currentLocation, char command)
        {
            (int x, int y) nextLocation;
            switch (command)
            {
                case 'N': nextLocation = (currentLocation.x, currentLocation.y - 1); break;
                case 'S': nextLocation = (currentLocation.x, currentLocation.y + 1); break;
                case 'E': nextLocation = (currentLocation.x + 1, currentLocation.y); break;
                case 'W': nextLocation = (currentLocation.x - 1, currentLocation.y); break;
                default: throw new InvalidOperationException($"Unknown command '{command}' found while navigating");
            }

            if (!maze.ContainsKey(nextLocation))
                maze[nextLocation] = Direction.None;

            switch (command)
            {
                case 'N': maze[currentLocation] |= Direction.North; maze[nextLocation] |= Direction.South; break;
                case 'S': maze[currentLocation] |= Direction.South; maze[nextLocation] |= Direction.North; break;
                case 'E': maze[currentLocation] |= Direction.East; maze[nextLocation] |= Direction.West; break;
                case 'W': maze[currentLocation] |= Direction.West; maze[nextLocation] |= Direction.East; break;
            }

            return nextLocation;
        }
    }
}
