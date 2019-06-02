using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day21 : AdventOfCode2017SolverBase
    {
        public override string Name => "Day 21: Fractal Art";

        private readonly char[,] _initialPattern = new char[,] {
            { '.', '#', '.' },
            { '.', '.', '#' },
            { '#', '#', '#' }
        };

        private Dictionary<char[,], char[,]> _2x2Rules = new Dictionary<char[,], char[,]>();
        private Dictionary<char[,], char[,]> _3x3Rules = new Dictionary<char[,], char[,]>();
        private int _interationCount;

        public Day21(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            string? iterationCount;
            do
            {
                iterationCount = IO.RequestInput("Iteration count?");
            } while (iterationCount == default || !Int32.TryParse(iterationCount, out _interationCount));

            var inputLines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var inputRules = inputLines
                .Select(p => p.Split("=>").Select(q => q.Trim()).ToArray())
                .Select(p => new
                {
                    input = p[0].Split("/").ToArray(),
                    output = p[1].Split("/").ToArray()
                })
                .ToList();

            _2x2Rules = inputRules.Where(p => p.input.Length == 2)
                .ToDictionary(p => p.input.ToCharArray(), p => p.output.ToCharArray());

            _3x3Rules = inputRules.Where(p => p.input.Length == 3)
                .ToDictionary(p => p.input.ToCharArray(), p => p.output.ToCharArray());
        }

        public override string ExecutePart1()
        {
            var Pos3s = new List2D<char>();
            var Pos2s = new List2D<char>();

            for (var i = 0; i < _initialPattern.GetLength(0); i++)
            {
                for (var j = 0; j < _initialPattern.GetLength(1); j++)
                    Pos3s[i, j] = _initialPattern[i, j];
            }

            var currentState = Pos3s;
            for (var iteration = 0; iteration < _interationCount; iteration++)
            {
                if (currentState == Pos3s)
                {
                    ApplyRules(2, _2x2Rules, currentState, Pos2s);
                    currentState = Pos2s;
                }
                else
                {
                    ApplyRules(3, _3x3Rules, currentState, Pos3s);
                    currentState = Pos3s;
                }

            }
            return "";
        }

        private void ApplyRules(int tileSize, Dictionary<char[,], char[,]> ruleSet, List2D<char> inputState, List2D<char> putputState)
        {

        }

        public override string ExecutePart2() => "";
    }

    public class List2D<T> where T : struct
    {
        private readonly List<List<T>> _backing = new List<List<T>>();

        public T this[int x, int y]
        {
            get
            {
                if (_backing.Count < y
                    || _backing[y].Count < x)
                {
                    return default;
                }

                return _backing[y][x];
            }
            set
            {
                while (_backing.Count < y)
                    _backing.Add(new List<T>());
                while (_backing[y].Count < x)
                    _backing[y].Add(default);
                _backing[y][x] = value;
            }
        }
    }
    public static class StringArrayExtensions
    {
        public static char[,] ToCharArray(this string[] input)
        {
            var result = new char[input.Length, input[0].Length];
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input.Length; j++)
                    result[i, j] = input[i][j];
            }

            return result;
        }
    }
}
