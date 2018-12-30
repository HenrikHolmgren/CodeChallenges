using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day15
{
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

        internal Actor AttackTargetAndReturn(IEnumerable<Actor> enemies)
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
            if (layout[Position.X, Position.Y - 1] == '.') yield return (Position.X, Position.Y - 1);
            if (layout[Position.X - 1, Position.Y] == '.') yield return (Position.X - 1, Position.Y);
            if (layout[Position.X + 1, Position.Y] == '.') yield return (Position.X + 1, Position.Y);
            if (layout[Position.X, Position.Y + 1] == '.') yield return (Position.X, Position.Y + 1);
        }

        internal void ApplyAttackBoost(int boost)
        {
            Attack += boost;
        }
    }
}
