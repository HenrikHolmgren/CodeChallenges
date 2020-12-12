using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day11 : AdventOfCode2020SolverBase
    {
        char[,] _initialState = new char[0, 0];

        public Day11(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 11: Seating System";
        public override string ExecutePart1() => RunAutomaton(StepAutomatronSimple);
        public override string ExecutePart2() => RunAutomaton(StepAutomatronFull);

        private string RunAutomaton(Action<char[,], char[,]> stepper)
        {
            var buffer = new char[_initialState.GetLength(0), _initialState.GetLength(1)];
            var state = new char[_initialState.GetLength(0), _initialState.GetLength(1)];
            for (int x = 0; x < _initialState.GetLength(0); x++)
                for (int y = 0; y < _initialState.GetLength(1); y++)
                    state[x, y] = _initialState[x, y];

            while (!StatesAreEqual(buffer, state))
            {
                stepper(state, buffer);
                var tmp = state;
                state = buffer;
                buffer = tmp;
            }

            var occupiedCount = 0;
            for (int x = 0; x < state.GetLength(0); x++)
                for (int y = 0; y < state.GetLength(1); y++)
                    if (state[x, y] == '#')
                        occupiedCount++;
            return occupiedCount.ToString();
        }

        private bool StatesAreEqual(char[,] buffer, char[,] state)
        {
            for (int x = 0; x < _initialState.GetLength(0); x++)
                for (int y = 0; y < _initialState.GetLength(1); y++)
                    if (buffer[x, y] != state[x, y]) return false;
            return true;
        }

        private void StepAutomatronSimple(char[,] source, char[,] target)
        {
            for (int x = 0; x < _initialState.GetLength(0); x++)
                for (int y = 0; y < _initialState.GetLength(1); y++)
                {
                    switch (source[x, y])
                    {
                        case 'L':
                            if (!Neighbourhood(x, y).Any(p => source[p.x, p.y] == '#'))
                                target[x, y] = '#';
                            else
                                target[x, y] = 'L';
                            break;
                        case '#':
                            if (Neighbourhood(x, y).Count(p => source[p.x, p.y] == '#') >= 4)
                                target[x, y] = 'L';
                            else
                                target[x, y] = '#';
                            break;
                        default:
                            target[x, y] = source[x, y];
                            break;
                    }
                }
        }

        private void StepAutomatronFull(char[,] source, char[,] target)
        {
            for (int x = 0; x < _initialState.GetLength(0); x++)
                for (int y = 0; y < _initialState.GetLength(1); y++)
                {
                    switch (source[x, y])
                    {
                        case 'L':
                            if (!GetVisibleSeats(x, y, source).Any(p => p == '#'))
                                target[x, y] = '#';
                            else
                                target[x, y] = 'L';
                            break;
                        case '#':
                            if (GetVisibleSeats(x, y, source).Count(p => p == '#') >= 5)
                                target[x, y] = 'L';
                            else
                                target[x, y] = '#';
                            break;
                        default:
                            target[x, y] = source[x, y];
                            break;
                    }
                }
        }

        private IEnumerable<char> GetVisibleSeats(int x, int y, char[,] source)
        {
            for (int xDir = -1; xDir <= 1; xDir++)
            {
                for (int yDir = -1; yDir <= 1; yDir++)
                {
                    if (xDir == yDir && xDir == 0) continue;

                    yield return BuildRay(x, y, xDir, yDir)
                        .Select(p => (char?)source[p.x, p.y])
                        .FirstOrDefault(p => p != '.')
                        ?? '.';
                }
            }
        }

        private IEnumerable<(int x, int y)> BuildRay(int x, int y, int xDir, int yDir)
        {
            var xPos = x + xDir;
            var yPos = y + yDir;
            while (xPos >= 0 && xPos < _initialState.GetLength(0)
                && yPos >= 0 && yPos < _initialState.GetLength(1))
            {
                yield return (xPos, yPos);
                xPos += xDir; yPos += yDir;
            }
        }

        public IEnumerable<(int x, int y)> Neighbourhood(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y) continue;
                    if (i < 0 || i >= _initialState.GetLength(0)) continue;
                    if (j < 0 || j >= _initialState.GetLength(1)) continue;
                    yield return (i, j);
                }
        }

        public override void Initialize(string input)
        {
            var inputLines = ReadAndSplitInput<string>(input).ToArray();
            _initialState = new char[inputLines[0].Length, inputLines.Length];
            for (int x = 0; x < inputLines[0].Length; x++)
                for (int y = 0; y < inputLines.Length; y++)
                    _initialState[x, y] = inputLines[y][x];
        }
    }

}
