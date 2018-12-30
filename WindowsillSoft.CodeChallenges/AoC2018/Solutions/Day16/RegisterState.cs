using System;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day16
{
    public struct RegisterState
    {
        public int R0;
        public int R1;
        public int R2;
        public int R3;

        public RegisterState(int[] state) =>
            (R0, R1, R2, R3) = (state[0], state[1], state[2], state[3]);

        public int this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        private void SetValue(int index, int value)
        {
            switch (index)
            {
                case 0: R0 = value; break;
                case 1: R1 = value; break;
                case 2: R2 = value; break;
                case 3: R3 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        private int GetValue(int index)
        {
            switch (index)
            {
                case 0: return R0;
                case 1: return R1;
                case 2: return R2;
                case 3: return R3;
                default: throw new IndexOutOfRangeException();
            }
        }

        public RegisterState WithValue(int value, int index)
        {
            var result = new RegisterState();

            (result.R0, result.R1, result.R2, result.R3) = (R0, R1, R2, R3);
            result[index] = value;

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is RegisterState reg)
                return Equals(reg);
            return false;
        }

        public bool Equals(RegisterState other)
        {
            return R0 == other.R0 &&
                R1 == other.R1 &&
                R2 == other.R2 &&
                R3 == other.R3;
        }
        public override int GetHashCode()
            => R0 ^ 17 * R1 ^ 19 * R2 ^ 23 * R3;

        public override string ToString()
            => $"(0:{R0} 1:{R1} 2:{R2} 3:{R3})";

    }

}
