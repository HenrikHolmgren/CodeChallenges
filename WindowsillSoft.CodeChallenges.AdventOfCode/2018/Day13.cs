using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day13 : AdventOfCode2018SolverBase
    {
        private char[,] _map = new char[0, 0];

        public Day13(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 13: Mine Cart Madness";

        public override void Initialize(string input)
        {
            var mapLines = ReadAndSplitInput<string>(input).ToArray();

            _map = new char[mapLines[0].Length, mapLines.Length];
            for (int y = 0; y < mapLines.Length; y++)
                for (int x = 0; x < mapLines[0].Length; x++)
                    _map[x, y] = mapLines[y][x];
        }

        public override string ExecutePart1()
        {
            var firstCollision = GetCollisions().First();
            return $"{firstCollision.x},{firstCollision.y}";
        }

        public override string ExecutePart2()
        {
            var lastCar = GetCollisions().Last();
            return $"{lastCar.x},{lastCar.y}";
        }

        private IEnumerable<(int x, int y)> GetCollisions()
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
                            case '<':
                                car.SetDirection(-1, 0);
                                break;
                            case '>':
                                car.SetDirection(1, 0);
                                break;
                            case '^':
                                car.SetDirection(0, -1);
                                break;
                            case 'v':
                                car.SetDirection(0, 1);
                                break;
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
                        IO.LogIfAttached(() => $"Collision at ({car.X}, {car.Y})");
                        yield return (car.X, car.Y);
                    }
                }
                foreach (var wreck in collidedCars)
                    cars.Remove(wreck);
            }
            IO.LogIfAttached(() => $"Final car: {cars.Single()}");
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

        public enum Direction
        {
            Left, Straight, Right,
        }

        public class MineCar
        {
            private Direction nextDirection = Direction.Left;

            public MineCar(int x, int y)
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
                        if (Vx != 0)
                            Rotate(Direction.Left);
                        else
                            Rotate(Direction.Right);
                        break;
                    case '\\':
                        if (Vx != 0)
                            Rotate(Direction.Right);
                        else
                            Rotate(Direction.Left);
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
    }
}
