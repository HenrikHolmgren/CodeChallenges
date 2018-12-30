using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day15
{
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
                if (actor.Health <= 0) continue; //Dead elves tell no tales

                if (verbose) actor.ToConsole(ConsoleColor.Yellow);

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

                if (verbose) Thread.Sleep(50);

                if (verbose) actor.ToConsole(ConsoleColor.Gray);
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

        private void HandleAttack(Actor defender)
        {
            if (defender == null) return;
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

        internal void PrintStats()
        {
            Console.WriteLine($"Battle finished in {StepCount} steps.");
            Console.WriteLine($"Scores are:");
            foreach (var group in Actors.GroupBy(p => p.Kind))
                Console.WriteLine($"{group.Key}: {group.Count(p => p.Health < 0)} dead, {group.Count(p => p.Health >= 0)} alive, for a board score of {group.Where(p => p.Health >= 0).Sum(p => p.Health) * (StepCount - 1)}");
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
