using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day13
{
    public class Day13Solver : IProblemSolver
    {
        public string Description => "Day 13";

        public int SortOrder => 13;

        public void Solve()
        {
            var input = File.ReadAllLines("Day13/Day13Input.txt");

            char[,] map = new char[input[0].Length, input.Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    map[x, y] = input[y][x];

            var cars = new List<Car>();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if ("<>^v".Contains(map[x, y]))
                    {
                        var car = new Car(x, y);
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
                var collidedCars = new List<Car>();
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
                        Console.WriteLine($"Collision at ({car.X}, {car.Y})");
                    }
                }
                foreach (var wreck in collidedCars)
                    cars.Remove(wreck);
            }

            Console.WriteLine($"Final car: {cars.Single()}");
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

    class Car
    {
        private Direction nextDirection = Direction.Left;

        public Car(int x, int y)
            => (X, Y) = (x, y);

        public int Vx { get; private set; }
        public int Vy { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public void Move(char nextTrack)
        {
            X += Vx;
            Y += Vy;

            switch (nextTrack)
            {
                case '/':
                    if (Vx != 0) Rotate(Direction.Left);
                    else Rotate(Direction.Right);
                    break;
                case '\\':
                    if (Vx != 0) Rotate(Direction.Right);
                    else Rotate(Direction.Left);
                    break;
                case '-':
                case '|':
                    break;
                case '+':
                    Turn();
                    break;
            }
        }

        private void Turn()
        {
            Rotate(nextDirection);
            switch (nextDirection)
            {
                case Direction.Left:
                    nextDirection = Direction.Straight;
                    break;
                case Direction.Straight:
                    nextDirection = Direction.Right;
                    break;
                case Direction.Right:
                    nextDirection = Direction.Left;
                    break;
            }
        }

        private void Rotate(Direction direction)
        {
            //Console.Write($"Turning {direction} from ({Vx}, {Vy}) ");
            if (direction == Direction.Straight)
                return;

            Vx ^= Vy;
            Vy ^= Vx;
            Vx ^= Vy;

            if (direction == Direction.Right)
                Vx *= -1;
            else
                Vy *= -1;
            //Console.WriteLine($"To ({Vx}, {Vy})");

        }

        internal void SetDirection(int vx, int vy)
        {
            (Vx, Vy) = (vx, vy);
        }

        public override string ToString()
            => $"{{({X},{Y}) ({Vx},{Vy})}}";
    }

    enum Direction
    {
        Left, Straight, Right,
    }
}
