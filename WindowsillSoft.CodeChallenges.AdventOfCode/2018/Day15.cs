using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day15 : AdventOfCode2018SolverBase
    {
        private string[] _mapLayout = new string[0];

        public Day15(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 15: Beverage Bandits";

        public override void Initialize(string input)
        {
            _mapLayout = input.Split('\n');
        }

        public override string ExecutePart1()
        {
            var map = Map.Parse(_mapLayout);
            IO.LogIfAttached(() => $"Part 1 - simulating battle...");

            while (map.Step(false))
                ;

            IO.LogIfAttached(() => map.PrintStats());

            return map.GetScore(verbose: false);
        }

        public override string ExecutePart2()
        {
            int boost = 1;
            IO.LogIfAttached(() => "Part 2 - seeking min boost...");

            //Do NOT try to use binary search here - cost me several hours only to find that at boost 13, elves all survive, but at 14, the goblins take one of them out again >_<
            while (true)
            {
                if (AnyElvesLost(boost))
                    boost++;
                else
                    break;
            }

            IO.LogIfAttached(() => $"Optimal boost: {boost}");

            var map = Map.Parse(_mapLayout);
            map.ApplyElfBonus(boost);
            while (map.Step(false))
                ;

            return map.GetScore(verbose: false);
        }

        private bool AnyElvesLost(int boost)
        {
            var map = Map.Parse(_mapLayout);
            map.ApplyElfBonus(boost);
            while (map.Step(false))
                ;

            if (!map.Actors.Any(p => p.Kind == 'E' && p.Health < 0))
            {
                IO.LogIfAttached(() => $"With a boost of {boost}, every elf survives the battle. Board score: {map.GetScore(verbose: true)}.");
                return false;
            }
            else
            {
                IO.LogIfAttached(() => $"With a boost of {boost}, {map.Actors.Where(p => p.Kind == 'E').Where(p => p.Health < 0).Count()} elves died :(");
                return true;
            }
        }

        public class Actor
        {
            public Actor(int x, int y, char kind)
                => (Position, Kind) = ((x, y), kind);

            public (int X, int Y) Position { get; set; }
            public char Kind { get; }
            public int Health { get; set; } = 200;
            public int Attack { get; private set; } = 3;

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

            internal Actor? AttackTargetAndReturn(IEnumerable<Actor> enemies)
            {
                var inRangeTarget = enemies
                    .Where(p => p.InRange(this))
                    .OrderBy(p => p.Health)
                    .ThenBy(p => p.Position.Y)
                    .ThenBy(p => p.Position.X)
                    .FirstOrDefault();

                if (inRangeTarget == null)
                    return null;

                inRangeTarget.Health -= Attack;
                return inRangeTarget;
            }

            public (int x, int y)? GetIntendedMove(IEnumerable<Actor> enemies, Map map)
            {
                var distances = map.GetDistances(Position);

                var intendedTarget = enemies.SelectMany(p => p.AttackLocations(map.Layout))
                    .OrderBy(p => distances[p.x, p.y])
                    .ThenBy(p => p.y)
                    .ThenBy(p => p.x)
                    .Cast<(int x, int y)?>()
                    .FirstOrDefault();

                if (intendedTarget == null || distances[intendedTarget.Value.x, intendedTarget.Value.y] == int.MaxValue)
                    return null;

                return map.FindPath(Position, intendedTarget.Value).Skip(1).First();
            }

            private IEnumerable<(int x, int y)> AttackLocations(char[,] layout)
            {
                if (layout[Position.X, Position.Y - 1] == '.')
                    yield return (Position.X, Position.Y - 1);
                if (layout[Position.X - 1, Position.Y] == '.')
                    yield return (Position.X - 1, Position.Y);
                if (layout[Position.X + 1, Position.Y] == '.')
                    yield return (Position.X + 1, Position.Y);
                if (layout[Position.X, Position.Y + 1] == '.')
                    yield return (Position.X, Position.Y + 1);
            }

            internal void ApplyAttackBoost(int boost)
            {
                Attack += boost;
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
                var actorOrder = Actors.Where(p => p.Health >= 0)
                    .OrderBy(p => p.Position.Y)
                    .ThenBy(p => p.Position.X)
                    .ToList();

                foreach (var actor in actorOrder)
                {
                    if (actor.Health <= 0)
                        continue; //Dead elves tell no tales

                    if (verbose)
                        actor.ToConsole(ConsoleColor.Yellow);

                    var enemies = actorOrder.Where(p => p.Kind != actor.Kind)
                        .Where(p => p.Health >= 0);

                    if (enemies.Count() == 0)
                        return false;

                    var hitEnemy = actor.AttackTargetAndReturn(enemies);
                    HandleAttack(hitEnemy);

                    if (hitEnemy == null)
                    {
                        var nextPosition = actor.GetIntendedMove(enemies, this);
                        if (nextPosition != null)
                            HandleMove(actor, nextPosition);

                        hitEnemy = actor.AttackTargetAndReturn(enemies);

                        HandleAttack(hitEnemy);
                    }

                    if (verbose)
                        Thread.Sleep(50);

                    if (verbose)
                        actor.ToConsole(ConsoleColor.Gray);
                }
                return true;
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

            private void HandleAttack(Actor? defender)
            {
                if (defender == null)
                    return;
                if (defender.Health < 0)
                    Layout[defender.Position.X, defender.Position.Y] = '.';
            }

            public void ToConsole(int left = 0, int top = 0, bool omitGround = false)
            {
                (Console.CursorTop, Console.CursorLeft) = (top, left);

                foreach (var y in Enumerable.Range(0, Layout.GetLength(1)))
                {
                    foreach (var x in Enumerable.Range(0, Layout.GetLength(0)))
                    {
                        if (!omitGround || Layout[x, y] != '.')
                            Console.Write(Layout[x, y]);
                        else
                            Console.CursorLeft++;
                    }
                    Console.WriteLine($" {String.Join(", ", Actors.Where(p => p.Position.Y == y).OrderBy(p => p.Position.X))}");
                }
            }

            internal string PrintStats()
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine($"Battle finished in {StepCount} steps.");
                result.AppendLine($"Scores are:");
                foreach (var group in Actors.GroupBy(p => p.Kind))
                    result.AppendLine($"{group.Key}: {group.Count(p => p.Health < 0)} dead, {group.Count(p => p.Health >= 0)} alive, for a board score of {group.Where(p => p.Health >= 0).Sum(p => p.Health) * (StepCount - 1)}");
                return result.ToString();
            }

            public string GetScore(bool verbose)
            {
                var winners = Actors.GroupBy(p => p.Kind).Select(p => new { Kind = p.Key, Score = p.Where(q => q.Health >= 0).Sum(q => q.Health) })
                    .OrderByDescending(p => p.Score)
                    .First();
                if (verbose)
                    return $"{winners.Kind} win after {StepCount - 1} rounds with {winners.Score} total hit points, and a board score of {winners.Score * (StepCount - 1)}.";
                else
                    return (winners.Score * (StepCount - 1)).ToString();
            }

            internal int[,] GetDistances((int X, int Y) start)
            {
                int[,] distances = new int[Layout.GetLength(0), Layout.GetLength(1)];
                foreach (var c in
                   from x in Enumerable.Range(0, distances.GetLength(0))
                   from y in Enumerable.Range(0, distances.GetLength(1))
                   select (x, y))
                    distances[c.x, c.y] = int.MaxValue;

                distances[start.X, start.Y] = 0;

                (int x, int y) current = default;

                var fringe = new List<(int x, int y)> { start };

                while (fringe.Any())
                {
                    current = fringe.First();

                    fringe.RemoveAt(0);
                    var dist = distances[current.x, current.y];
                    foreach (var candidate in new[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
                        if (Layout[current.x + candidate.Item1, current.y + candidate.Item2] == '.')
                        {
                            if (distances[current.x + candidate.Item1, current.y + candidate.Item2] != int.MaxValue)
                                continue;

                            fringe.Add((current.x + candidate.Item1, current.y + candidate.Item2));
                            distances[current.x + candidate.Item1, current.y + candidate.Item2] = dist + 1;
                        }
                }

                return distances;
            }

            internal IEnumerable<(int x, int y)> FindPath((int X, int Y) start, (int x, int y) target)
            {
                var returnDistances = GetDistances(target);
                var position = start;
                while (position != target)
                {
                    yield return position;
                    var next = position;
                    var bestDist = Int32.MaxValue;

                    foreach (var candidate in new[] { (0, 1), (1, 0), (-1, 0), (0, -1) })
                        if (returnDistances[position.X + candidate.Item1, position.Y + candidate.Item2] <= returnDistances[next.X, next.Y])
                        {
                            next = (position.X + candidate.Item1, position.Y + candidate.Item2);
                            bestDist = returnDistances[next.X, next.Y];
                        }
                    if (next != position)
                        position = next;
                    else
                        throw new InvalidOperationException("No next move found!");
                }
                yield return target;
            }

            public void ApplyElfBonus(int boost)
            {
                foreach (var elf in Actors.Where(p => p.Kind == 'E'))
                    elf.ApplyAttackBoost(boost);
            }
        }
    }
}
