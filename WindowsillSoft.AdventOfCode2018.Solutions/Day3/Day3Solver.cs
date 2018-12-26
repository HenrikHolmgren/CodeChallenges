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

            var input = File.ReadAllLines("Day3/Day3Input.txt");

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

            //int minX = claims.Min(p => p.X);
            //int minY = claims.Min(p => p.Y);
            //int maxX = claims.Max(p => p.X + p.Width);
            //int maxY = claims.Max(p => p.Y + p.Height);
            //int count = 0;
            //for (int x = minX; x < maxX; x++)
            //    for (int y = minY; y < maxY; y++)
            //    {
            //        if (claims.Count(p => p.Contains(x, y)) > 1)
            //            count++;
            //    }
            //Console.WriteLine($"{count} overlaps.");

            //for (int i = 0; i < claims.Length - 1; i++)
            //{
            //    if (!claims.Any(p => p.Id != claims[i].Id && p.Overlaps(claims[i])))
            //        Console.WriteLine(claims[i].Id);
            //}

            int[,] reservedAreas = new int[claims.Max(p => p.X + p.Width), claims.Max(p => p.Y + p.Height)];

            HashSet<Claim> nonOverlapping = new HashSet<Claim>(claims);
            var dicClaims = claims.ToDictionary(p => p.Id);

            int overlaps = 0;

            foreach (var claim in claims)
            {
                for (int x = claim.X; x < claim.X + claim.Width; x++)
                {
                    for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                    {
                        if (reservedAreas[x, y] != 0)
                        {
                            if (nonOverlapping.Contains(claim))
                                nonOverlapping.Remove(claim);
                            if (reservedAreas[x, y] != -1)
                            {
                                if (nonOverlapping.Contains(dicClaims[reservedAreas[x, y]]))
                                    nonOverlapping.Remove(dicClaims[reservedAreas[x, y]]);

                                reservedAreas[x, y] = -1;
                                overlaps++;
                            }
                        }
                        else
                            reservedAreas[x, y] = claim.Id;
                    }
                }
            }

            Console.WriteLine($"After placement, a total of {overlaps} squares had overlaps, and claim id(s) {String.Join(", ", nonOverlapping.Select(p => p.Id))} were not overlapped.");
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
