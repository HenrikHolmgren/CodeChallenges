using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day10 : AdventOfCode2018SolverBase
    {
        private Star[] _stars = new Star[0];

        public Day10(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 10: The Stars Align";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"position=<(?'x'[^,]*),(?'y'[^>]*)> velocity=<(?'vx'[^,]*),(?'vy'[^>]*)>", RegexOptions.Compiled);

            _stars = input.Split('\n')
                .Select(p => matcher.Match(p))
                .Select(p => new Star
                {
                    X = int.Parse(p.Groups["x"].Value),
                    Y = int.Parse(p.Groups["y"].Value),
                    Vx = int.Parse(p.Groups["vx"].Value),
                    Vy = int.Parse(p.Groups["vy"].Value),
                }).ToArray();
        }

        public override string ExecutePart1()
        {
            var constellation = BuildBestConstellation();
            return DumpFrame(constellation.Constellation);
        }

        public override string ExecutePart2()
        {
            var constellation = BuildBestConstellation();
            return constellation.Timestamp.ToString();
        }

        private (int Timestamp, Star[] Constellation) BuildBestConstellation()
        {
            var Frames = new List<(long Area, Star[] Constellation)>();
            long xSize = _stars.Max(p => p.X) - _stars.Min(p => p.X);
            long ySize = _stars.Max(p => p.Y) - _stars.Min(p => p.Y);
            Frames.Add((Math.Abs(xSize * ySize), _stars));

            var current = _stars.ToArray();

            while (Frames.Last().Area <= Frames.First().Area)
            {
                Star[] newFrame = new Star[current.Length];
                for (int i = 0; i < current.Length; i++)
                {
                    var star = current[i];
                    star.X += star.Vx;
                    star.Y += star.Vy;
                    newFrame[i] = star;
                }

                xSize = newFrame.Max(p => p.X) - newFrame.Min(p => p.X);
                ySize = newFrame.Max(p => p.Y) - newFrame.Min(p => p.Y);
                Frames.Add((xSize * ySize, newFrame));
                current = newFrame;
            }

            var bestFrame = Frames
                .OrderBy(p => p.Area)
                .First();

            return (Frames.IndexOf(bestFrame), bestFrame.Constellation);
        }

        private string DumpFrame(Star[] frame)
        {
            var offsetX = frame.Min(p => p.X);
            var offsetY = frame.Min(p => p.Y);

            StringBuilder result = new StringBuilder();
            var maxX = frame.Max(p => p.X);
            int currentX = offsetX;
            int currentY = offsetY;

            foreach (var star in frame
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .Distinct())
            {
                while (star.Y > currentY)
                {
                    result.Append('.', maxX - currentX);
                    result.AppendLine();
                    currentX = offsetX;
                    currentY++;
                }

                if (star.X > currentX)
                    result.Append('.', star.X - currentX - 1);

                result.Append("#");
                currentX = star.X;
            }

            return result.ToString();
        }

        public struct Star
        {
            public int X;
            public int Y;
            public int Vx;
            public int Vy;

            public override bool Equals(object? obj)
            {
                return (obj is Star star
                    && star.X == X
                    && star.Y == Y);
            }

            public override int GetHashCode()
            {
                return X ^ 17 * Y ^ 19;
            }
        }
    }
}