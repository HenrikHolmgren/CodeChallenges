using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day03 : AdventOfCode2018SolverBase
    {
        public override string Name => "Day 3: No Matter How You Slice It";

        private Claim[] _claims = new Claim[0];

        public Day03(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"#(?'id'\d+)\s+@\s+(?'x'\d+),(?'y'\d+):\s+(?'width'\d+)x(?'height'\d+)",
                RegexOptions.Compiled);

            _claims = input
                .Split('\n')
                .Select(p => matcher.Match(p))
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
        }

        public override string ExecutePart1()
        {
            //TODO: Something less terrible than this .. :\

            int minX = _claims.Min(p => p.X);
            int minY = _claims.Min(p => p.Y);
            int maxX = _claims.Max(p => p.X + p.Width);
            int maxY = _claims.Max(p => p.Y + p.Height);
            int count = 0;
            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                {
                    if (_claims.Count(p => p.Contains(x, y)) > 1)
                        count++;
                }

            return count.ToString();
        }

        public override string ExecutePart2()
        {
            int[,] reservedAreas = new int[_claims.Max(p => p.X + p.Width), _claims.Max(p => p.Y + p.Height)];

            HashSet<Claim> nonOverlapping = new HashSet<Claim>(_claims);
            var dicClaims = _claims.ToDictionary(p => p.Id);

            int overlaps = 0;

            foreach (var claim in _claims)
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
            return String.Join(", ", nonOverlapping.Select(p => p.Id));
        }

        public struct Claim
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
