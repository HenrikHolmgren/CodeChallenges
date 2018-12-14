using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day3
{
    public class Day3Solver : IProblemSolver
    {
        public string Description => "Day 3";

        public int SortOrder => 3;

        public void Solve()
        {
            var matcher = new Regex(@"#(?'id'\d+)\s+@\s+(?'x'\d+),(?'y'\d+):\s+(?'width'\d+)x(?'height'\d+)",
                RegexOptions.Compiled);

            var input = new[]
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2",
            };

            //var input = File.ReadAllLines("Day3/Day3Input.txt");

            var claims = input.Select(p => matcher.Match(p))
                .Select(p => new Claim
                {
                    Id = int.Parse(p.Groups["id"].Value),
                    X = int.Parse(p.Groups["x"].Value),
                    Y = int.Parse(p.Groups["y"].Value),
                    Width = int.Parse(p.Groups["width"].Value),
                    Height = int.Parse(p.Groups["height"].Value),
                })
                .OrderBy(p => p.X)
                .ThenBy(p => p.Y)
                .ThenBy(p => p.Width)
                .ThenBy(p => p.Height)
                .ToArray();

            int minX = claims.Min(p => p.X);
            int minY = claims.Min(p => p.Y);
            int maxX = claims.Max(p => p.X + p.Width);
            int maxY = claims.Max(p => p.Y + p.Height);
            int count = 0;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                {
                    if (claims.Count(p => p.Contains(x, y)) > 1)
                        count++;
                }
            Console.WriteLine($"{count} overlaps.");

            for (int i = 0; i < claims.Length - 1; i++)
            {
                if (!claims.Any(p => p.Id != claims[i].Id && p.Overlaps(claims[i])))
                    Console.WriteLine(claims[i].Id);
            }
        }

        internal struct Claim
        {
            public int Id;
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public int X2 => X + Width;
            public int Y2 => Y + Height;

            public bool Contains(int x, int y)
                => x >= X && x < X + Width
                && y >= Y && y < Y + Height;

            public bool Overlaps(Claim c2) =>
                Contains(c2.X, c2.Y) ||
                Contains(c2.X2, c2.Y) ||
                Contains(c2.X2, c2.Y2) ||
                Contains(c2.X, c2.Y2);

        }
    }
}
