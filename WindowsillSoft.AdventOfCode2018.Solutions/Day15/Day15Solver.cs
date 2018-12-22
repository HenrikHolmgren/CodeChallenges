using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day15
{
    public class Day15Solver : IProblemSolver
    {
        public string Description => "Day 15";

        public int SortOrder => 15;

        public void Solve()
        {
            Console.CursorVisible = false;
            var input = File.ReadAllLines("Day15/Day15Test4.txt");
            var map = Map.Parse(input);
            map.ToConsole(0, 0);

            while (map.Step(false)) ;

            map.ToConsole(0, 0);

            map.PrintStats();
            Console.CursorVisible = true;
        }
    }

    public class Map
    {
        private List<Actor> _Actors = new List<Actor>();
        public IReadOnlyList<Actor> Actors => _Actors.AsReadOnly();
        public char[,] Layout;
        public long StepCount { get; private set; }

        public static Map Parse(string[] mapLayout)
    => new Map(mapLayout);

        private Map(string[] mapLayout)
        {
            Layout = new char[mapLayout[0].Length, mapLayout.Length];

            foreach (var c in
                from y in Enumerable.Range(0, mapLayout.Length)
                from x in Enumerable.Range(0, mapLayout[0].Length)
                select (x, y))
            {
                Layout[c.x, c.y] = mapLayout[c.y][c.x];
                if (new[] { 'G', 'E' }.Contains(Layout[c.x, c.y]))
                    _Actors.Add(new Actor(c.x, c.y, Layout[c.x, c.y]));
            }
        }

        public bool Step(bool verbose = false)
        {
            StepCount++;

            if (verbose)
            {
                Console.Clear();
                ToConsole(0, 0);
            }

            var actorOrder = Actors.Where(p => p.Health >= 0).OrderBy(p => p.Position.Y).ThenBy(p => p.Position.X).ToList();
            foreach (var actor in actorOrder)
            {
                if (verbose) actor.ToConsole(ConsoleColor.Yellow);

                var enemies = actorOrder.Where(p => p.Kind != actor.Kind);
                var hitEnemy = actor.AttackTargetAndReturn(enemies);
                HandleAttack(hitEnemy);

                if (hitEnemy == null)
                {
                    var nextPosition = actor.GetIntendedMove(enemies, this);
                    HandleMove(actor, nextPosition);

                    hitEnemy = actor.AttackTargetAndReturn(enemies);
                    HandleAttack(hitEnemy);
                }

                if (verbose) Thread.Sleep(50);

                if (verbose) actor.ToConsole(ConsoleColor.Gray);
            }
            return !Actors.GroupBy(p => p.Kind).Any(p => p.All(q => q.Health < 0));
        }

        private void HandleMove(Actor actor, (int x, int y)? nextPosition)
        {
            if (nextPosition != null)
            {
                Layout[actor.Position.X, actor.Position.Y] = '.';
                Layout[nextPosition.Value.x, nextPosition.Value.y] = actor.Kind;
                actor.Position = (nextPosition.Value.x, nextPosition.Value.y);
            }
        }

        private void HandleAttack(Actor defender)
        {
            if (defender == null) return;
            if (defender.Health < 0)
                Layout[defender.Position.X, defender.Position.Y] = '.';
        }

        public void ToConsole(int left = 0, int top = 0, bool omitGround = false)
        {
            (Console.CursorTop, Console.CursorLeft) = (top, left);

            foreach (var y in Enumerable.Range(0, Layout.GetLength(0)))
            {
                foreach (var x in Enumerable.Range(0, Layout.GetLength(1)))
                {
                    if (!omitGround || Layout[x, y] != '.')
                        Console.Write(Layout[x, y]);
                    else
                        Console.CursorLeft++;
                }
                Console.WriteLine($" {String.Join(", ", Actors.Where(p => p.Position.Y == y).OrderBy(p => p.Position.X))}");
            }
        }

        internal (int x, int y)? FindNextMove((int x, int y) start, char targetKind)
        {

            int[,] outPaths = new int[Layout.GetLength(0), Layout.GetLength(1)];
            int[,] inPaths = new int[Layout.GetLength(0), Layout.GetLength(1)];
            foreach (var c in
               from x in Enumerable.Range(0, outPaths.GetLength(0))
               from y in Enumerable.Range(0, outPaths.GetLength(1))
               select (x, y))
                outPaths[c.x, c.y] = int.MaxValue;

            outPaths[start.x, start.y] = 0;

            (int x, int y) current = default;

            var fringe = new List<(int x, int y)> { start };
            while (fringe.Any())
            {
                current = fringe.First();

                fringe.RemoveAt(0);
                var dist = outPaths[current.x, current.y];
                foreach (var candidate in new[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
                    if (new[] { targetKind, '.' }.Contains(Layout[current.x + candidate.Item1,
                        current.y + candidate.Item2]))
                    {
                        if (outPaths[current.x + candidate.Item1, current.y + candidate.Item2] != int.MaxValue)
                            continue;
                        fringe.Add((current.x + candidate.Item1, current.y + candidate.Item2));
                        outPaths[current.x + candidate.Item1, current.y + candidate.Item2] = dist + 1;

                        if (Layout[current.x + candidate.Item1, current.y + candidate.Item2] == targetKind)
                        {
                            current = (current.x + candidate.Item1, current.y + candidate.Item2);
                            fringe.Clear();
                            break;
                        }
                    }
            }

            if (Layout[current.x, current.y] != targetKind)
                return null;

            fringe.Add(current);
            while (fringe.Any())
            {
                current = fringe.First();
                fringe.RemoveAt(0);

                inPaths[current.x, current.y] = outPaths[current.x, current.y];

                foreach (var candidate in new[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
                    if (outPaths[current.x + candidate.Item1, current.y + candidate.Item2] < outPaths[current.x, current.y])
                        fringe.Add((current.x + candidate.Item1, current.y + candidate.Item2));
            }

            current = start;
            while (Layout[current.x, current.y] != targetKind)
            {
                if (inPaths[current.x, current.y - 1] > inPaths[current.x, current.y]) current = (current.x, current.y - 1);
                else if (inPaths[current.x - 1, current.y] > inPaths[current.x, current.y]) current = (current.x - 1, current.y);
                else if (inPaths[current.x + 1, current.y] > inPaths[current.x, current.y]) current = (current.x + 1, current.y);
                else current = (current.x, current.y + 1);

                return current;
            }

            return null;
        }

        internal void PrintStats()
        {
            Console.Clear();
            ToConsole(0, 0);
            (Console.CursorTop, Console.CursorLeft) = (Layout.GetLength(1) + 1, 0);

            Console.WriteLine($"Current iteration is {StepCount}.");
            Console.WriteLine($"Scores are:");
            foreach (var group in Actors.GroupBy(p => p.Kind))
                Console.WriteLine($"{group.Key}: {group.Count(p => p.Health < 0)} dead, {group.Count(p => p.Health >= 0)} alive, for a board score of {group.Where(p => p.Health >= 0).Sum(p => p.Health) * StepCount}");
        }
    }

    public class Actor
    {
        public Actor(int x, int y, char kind)
            => (Position, Kind) = ((x, y), kind);

        public (int X, int Y) Position { get; set; }
        public char Kind { get; }
        public int Health { get; set; } = 200;
        public int Attack => 3;

        public override string ToString()
            => $"{Kind}({Health})";

        internal void ToConsole(ConsoleColor actorColor)
        {
            var tmp = Console.ForegroundColor;
            Console.ForegroundColor = actorColor;
            (Console.CursorLeft, Console.CursorTop) = Position;
            Console.Write(Kind);
            Console.ForegroundColor = tmp;
        }

        public bool InRange(Actor other)
            => Math.Abs(other.Position.X - Position.X) + Math.Abs(other.Position.Y - Position.Y) == 1;

        internal (int x, int y)? GetAttackLocation(char[,] layout)
        {
            if (layout[Position.X, Position.Y - 1] == '.') return (Position.X, Position.Y - 1);
            if (layout[Position.X - 1, Position.Y] == '.') return (Position.X - 1, Position.Y);
            if (layout[Position.X + 1, Position.Y] == '.') return (Position.X + 1, Position.Y);
            if (layout[Position.X, Position.Y + 1] == '.') return (Position.X, Position.Y + 1);
            return null;
        }

        internal Actor AttackTargetAndReturn(IEnumerable<Actor> enemies)
        {
            var inRangeTarget = enemies
                .Where(p => p.InRange(this))
                .OrderBy(p => p.Health)
                .FirstOrDefault();

            if (inRangeTarget == null)
                return null;

            inRangeTarget.Health -= Attack;
            return inRangeTarget;
        }

        public (int x, int y)? GetIntendedMove(IEnumerable<Actor> enemies, Map map)
            => map.FindNextMove(Position, Kind == 'E' ? 'G' : 'E');
    }
}
