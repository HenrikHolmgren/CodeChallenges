using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day13
{
    public class Day13Solver : AoC2018SolverBase
    {
        private char[,] _map;

        public override string Description => "Day 13: Mine Cart Madness";

        public override int SortOrder => 13;

        public override void Initialize(string input)
        {
            var mapLines = input.Split(Environment.NewLine);

            _map = new char[mapLines[0].Length, mapLines.Length];
            for (int y = 0; y < mapLines.Length; y++)
                for (int x = 0; x < mapLines[0].Length; x++)
                    _map[x, y] = mapLines[y][x];
        }

        public override string SolvePart1(bool silent = true)
        {
            var firstCollision = GetCollisions(silent).First();
            return $"{firstCollision.x},{firstCollision.y}";
        }

        public override string SolvePart2(bool silent = true)
        {
            var lastCar = GetCollisions(silent).Last();
            return $"{lastCar.x},{lastCar.y}";
        }

        private IEnumerable<(int x, int y)> GetCollisions(bool silent)
        {
            var cars = new List<MineCar>();
            var map = (char[,])_map.Clone();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if ("<>^v".Contains(map[x, y]))
                    {
                        var car = new MineCar(x, y);
                        switch (map[x, y])
                        {
                            case '<': car.SetDirection(-1, 0); break;
                            case '>': car.SetDirection(1, 0); break;
                            case '^': car.SetDirection(0, -1); break;
                            case 'v': car.SetDirection(0, 1); break;
                        }
                        if ("<>".Contains(map[x, y]))
                            map[x, y] = '-';
                        else
                            map[x, y] = '|';
                        cars.Add(car);
                    }
                }
            }

            while (cars.Count > 1)
            {
                var collidedCars = new List<MineCar>();
                foreach (var car in cars.OrderBy(p => p.Y).ThenBy(p => p.X).ToList())
                {
                    if (collidedCars.Contains(car))
                        continue;

                    car.Move(map[car.X + car.Vx, car.Y + car.Vy]);
                    var doublet = cars.FirstOrDefault(p => p != car && p.X == car.X && p.Y == car.Y);
                    if (doublet != null)
                    {
                        collidedCars.Add(car);
                        collidedCars.Add(doublet);
                        if (!silent)
                            Console.WriteLine($"Collision at ({car.X}, {car.Y})");
                        yield return (car.X, car.Y);
                    }
                }
                foreach (var wreck in collidedCars)
                    cars.Remove(wreck);
            }
            if (!silent)
                Console.WriteLine($"Final car: {cars.Single()}");
            yield return (cars.Single().X, cars.Single().Y);
        }

        private static void WriteMap(char[,] map)
        {
            for (int line = 0; line < map.GetLength(1); line++)
            {
                for (int pos = 0; pos < map.GetLength(0); pos++)
                {
                    Console.Write(map[pos, line]);
                }
                Console.WriteLine();
            }
        }
    }
}
