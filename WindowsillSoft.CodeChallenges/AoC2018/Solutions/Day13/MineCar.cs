namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day13
{
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
}
