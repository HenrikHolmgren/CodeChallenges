using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day20 : AdventOfCode2018SolverBase
    {
        private Dictionary<(int x, int y), Direction> _maze = new Dictionary<(int x, int y), Direction>();

        public Day20(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 20: A Regular Map";

        public override void Initialize(string input)
        {
            var location = (x: 0, y: 0);
            _maze[location] = Direction.None;

            var BranchPoints = new Stack<(int x, int y)>();
            foreach (var command in input)
            {
                switch (command)
                {
                    case '^': /*Check for index = 0?*/
                        break;
                    case '$': /*Check that no other commands are enqueued?*/
                        break;
                    case '(':
                        BranchPoints.Push(location);
                        break;
                    case '|':
                        location = BranchPoints.Peek();
                        break;
                    case ')':
                        location = BranchPoints.Pop();
                        break;
                    default:
                        location = HandleNavigationDirection(_maze, location, command);
                        break;
                }
            }
        }

        public override string ExecutePart1()
        {
            var (distances, finalLocation) = SolveMaze();

            IO.LogIfAttached(() => $"Explortation stopped at {finalLocation} with distance {distances[finalLocation.x, finalLocation.y] - 1}.");

            return (distances[finalLocation.x, finalLocation.y] - 1).ToString();
        }

        public override string ExecutePart2()
        {
            var (distances, finalLocation) = SolveMaze();

            var longDistances = (
                from x in Enumerable.Range(0, distances.GetLength(0))
                from y in Enumerable.Range(0, distances.GetLength(1))
                select (x, y))
                .Count(p => distances[p.x, p.y] > 1000);

            return $"{longDistances}";
        }

        private (int[,] Distances, (int x, int y) FinalLocation) SolveMaze()
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
            var nextLocation = command switch
            {
                'N' => (currentLocation.x, currentLocation.y - 1),
                'S' => (currentLocation.x, currentLocation.y + 1),
                'E' => (currentLocation.x + 1, currentLocation.y),
                'W' => (currentLocation.x - 1, currentLocation.y),
                _ => throw new InvalidOperationException($"Unknown command '{command}' found while navigating"),
            };
            if (!maze.ContainsKey(nextLocation))
                maze[nextLocation] = Direction.None;

            switch (command)
            {
                case 'N':
                    maze[currentLocation] |= Direction.North;
                    maze[nextLocation] |= Direction.South;
                    break;
                case 'S':
                    maze[currentLocation] |= Direction.South;
                    maze[nextLocation] |= Direction.North;
                    break;
                case 'E':
                    maze[currentLocation] |= Direction.East;
                    maze[nextLocation] |= Direction.West;
                    break;
                case 'W':
                    maze[currentLocation] |= Direction.West;
                    maze[nextLocation] |= Direction.East;
                    break;
            }

            return nextLocation;
        }

        [Flags]
        public enum Direction : byte
        {
            None = 0b0000,
            North = 0b0001,
            South = 0b0010,
            East = 0b0100,
            West = 0b1000,
        }
    }

    public class Opcode
    {
        public Opcode(string name, Func<RegisterState, OpcodeParameterSet, RegisterState> doWork)
        {
            Name = name;
            DoWork = doWork;
        }

        public string Name { get; }
        public Func<RegisterState, OpcodeParameterSet, RegisterState> DoWork { get; }
    }

    public struct OpcodeParameterSet

    {
        public int A;
        public int B;
        public int C;

        public OpcodeParameterSet(int[] state) =>
           (A, B, C) = (state[0], state[1], state[2]);

        public int this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        private void SetValue(int index, int value)
        {
            switch (index)
            {
                case 0: A = value; break;
                case 1: B = value; break;
                case 2: C = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        private int GetValue(int index)
        {
            return index switch
            {
                0 => A,
                1 => B,
                2 => C,
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public override string ToString()
            => $"(A:{A} B:{B} C:{C})";
    }

    public struct RegisterState
    {
        public int R0;
        public int R1;
        public int R2;
        public int R3;
        public int R4;
        public int R5;

        public RegisterState(int[] state) =>
            (R0, R1, R2, R3, R4, R5) = (state[0], state[1], state[2], state[3], state[4], state[5]);

        public int this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        private void SetValue(int index, int value)
        {
            switch (index)
            {
                case 0: R0 = value; break;
                case 1: R1 = value; break;
                case 2: R2 = value; break;
                case 3: R3 = value; break;
                case 4: R4 = value; break;
                case 5: R5 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        private int GetValue(int index)
        {
            return index switch
            {
                0 => R0,
                1 => R1,
                2 => R2,
                3 => R3,
                4 => R4,
                5 => R5,
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public RegisterState WithValue(int value, int index)
        {
            var result = new RegisterState();

            (result.R0, result.R1, result.R2, result.R3, result.R4, result.R5) = (R0, R1, R2, R3, R4, R5);
            result[index] = value;

            return result;
        }

        public override bool Equals(object? obj)
        {
            if (obj is RegisterState reg)
                return Equals(reg);
            return false;
        }

        public bool Equals(RegisterState other)
        {
            return R0 == other.R0 &&
                R1 == other.R1 &&
                R2 == other.R2 &&
                R3 == other.R3 &&
                R4 == other.R4 &&
                R5 == other.R5;
        }

        public override int GetHashCode()
        {
            var res = 0;
            for (int i = 0; i < 6; i++)
                res = res * 13 + this[i].GetHashCode();
            return res;
        }

        public override string ToString()
            => $"(R0:{R0} R1:{R1} R2:{R2} R3:{R3} R4:{R4} R5:{R5})";

    }
}

