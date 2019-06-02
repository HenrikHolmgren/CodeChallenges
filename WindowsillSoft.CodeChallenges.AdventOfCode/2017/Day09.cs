using System;
using System.Collections.Generic;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day09 : AdventOfCode2017SolverBase
    {
        private string _stream;

        public Day09(IIOProvider provider) : base(provider) => _stream = String.Empty;

        public override string Name => "Day 9: Stream Processing";

        public override void Initialize(string input) => _stream = input;

        public override string ExecutePart1()
        {
            var currentGroup = 0;
            var groupSum = 0;

            var stateMemory = new Stack<State>();
            stateMemory.Push(State.Group);

            for (var i = 0; i < _stream.Length; i++)
            {
                var currentState = stateMemory.Peek();
                if (currentState == State.Comment)
                {
                    stateMemory.Pop();
                    continue;
                }

                switch (_stream[i])
                {
                    case '<' when currentState == State.Group:
                        stateMemory.Push(State.Garbage);
                        break;
                    case '>' when currentState == State.Garbage:
                        stateMemory.Pop();
                        break;
                    case '{' when currentState == State.Group:
                        currentGroup++;
                        stateMemory.Push(State.Group);
                        break;
                    case '}' when currentState == State.Group:
                        groupSum += currentGroup;
                        currentGroup--;
                        stateMemory.Pop();
                        break;
                    case '!':
                        stateMemory.Push(State.Comment);
                        break;
                    //default:
                    //    throw new InvalidOperationException($"Invalid transition {currentState}:{_stream[i]}!");
                }
            }

            return groupSum.ToString();
        }

        public override string ExecutePart2()
        {
            var index = 0;
            var garbageCount = 0;

            while (index < _stream.Length)
            {
                while (index < _stream.Length && _stream[index] != '<')
                    index++;
                index++;
                while (index < _stream.Length && _stream[index] != '>')
                {
                    if (_stream[index] == '!')
                        index++;
                    else
                        garbageCount++;
                    index++;
                }
            }

            return garbageCount.ToString();
        }

        private enum State
        {
            Group,
            Garbage,
            Comment
        }
    }
}
