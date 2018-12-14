using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day10
{
    public class Day10Solver : IProblemSolver
    {
        public string Description => "Day 10";

        public int SortOrder => 10;

        public void Solve()
        {
            var matcher = new Regex(@"position=<(?'x'[^,]*),(?'y'[^>]*)> velocity=<(?'vx'[^,]*),(?'vy'[^>]*)>", RegexOptions.Compiled);

            //            var input = @"position=< 9,  1> velocity=< 0,  2>
            //position=< 7,  0> velocity=<-1,  0>
            //position=< 3, -2> velocity=<-1,  1>
            //position=< 6, 10> velocity=<-2, -1>
            //position=< 2, -4> velocity=< 2,  2>
            //position=<-6, 10> velocity=< 2, -2>
            //position=< 1,  8> velocity=< 1, -1>
            //position=< 1,  7> velocity=< 1,  0>
            //position=<-3, 11> velocity=< 1, -2>
            //position=< 7,  6> velocity=<-1, -1>
            //position=<-2,  3> velocity=< 1,  0>
            //position=<-4,  3> velocity=< 2,  0>
            //position=<10, -3> velocity=<-1,  1>
            //position=< 5, 11> velocity=< 1, -2>
            //position=< 4,  7> velocity=< 0, -1>
            //position=< 8, -2> velocity=< 0,  1>
            //position=<15,  0> velocity=<-2,  0>
            //position=< 1,  6> velocity=< 1,  0>
            //position=< 8,  9> velocity=< 0, -1>
            //position=< 3,  3> velocity=<-1,  1>
            //position=< 0,  5> velocity=< 0, -1>
            //position=<-2,  2> velocity=< 2,  0>
            //position=< 5, -2> velocity=< 1,  2>
            //position=< 1,  4> velocity=< 2,  1>
            //position=<-2,  7> velocity=< 2, -2>
            //position=< 3,  6> velocity=<-1, -1>
            //position=< 5,  0> velocity=< 1,  0>
            //position=<-6,  0> velocity=< 2,  0>
            //position=< 5,  9> velocity=< 1, -2>
            //position=<14,  7> velocity=<-2,  0>
            //position=<-3,  6> velocity=< 2, -1>".Split(Environment.NewLine)
            var input = File.ReadAllLines("Day10/Day10Input.txt")
                .Select(p => matcher.Match(p))
                .Select(p => new Star
                {
                    X = int.Parse(p.Groups["x"].Value),
                    Y = int.Parse(p.Groups["y"].Value),
                    Vx = int.Parse(p.Groups["vx"].Value),
                    Vy = int.Parse(p.Groups["vy"].Value),
                }).ToArray();

            List<(long, Star[])> Frames = new List<(long, Star[])>();
            long xSize = input.Max(p => p.X) - input.Min(p => p.X);
            long ySize = input.Max(p => p.Y) - input.Min(p => p.Y);
            Frames.Add((Math.Abs(xSize * ySize), input));

            var current = input.ToArray();

            while (Frames.Last().Item1 <= Frames.First().Item1)
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

            var bestFrame = Frames.OrderBy(p => p.Item1).First();
            DumpFrame(bestFrame.Item2);

            Console.WriteLine(Frames.IndexOf(bestFrame));
        }

        private void DumpFrame(Star[] frame)
        {
            Console.Clear();
            var offsetX = frame.Min(p => p.X);
            var offsetY = frame.Min(p => p.Y);
            foreach (var star in frame)
            {
                Console.CursorLeft = star.X - offsetX;
                Console.CursorTop = star.Y - offsetY;
                Console.Write('*');
            }
            Console.CursorLeft = 0;
            Console.CursorTop = frame.Max(p => p.Y) - offsetY + 1;
        }

    }
    public struct Star
    {
        public int X;
        public int Y;
        public int Vx;
        public int Vy;
    }
}
