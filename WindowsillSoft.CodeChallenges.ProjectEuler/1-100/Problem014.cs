using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Numerics;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * The following iterative sequence is defined for the set of positive integers:
     * 
     * n → n/2 (n is even)
     * n → 3n + 1 (n is odd)
     * 
     * Using the rule above and starting with 13, we generate the following sequence:
     * 
     * 13 → 40 → 20 → 10 → 5 → 16 → 8 → 4 → 2 → 1
     * It can be seen that this sequence (starting at 13 and finishing at 1) contains 10 terms. Although it has not been proved yet (Collatz Problem), it is thought that all starting numbers finish at 1.
     * 
     * Which starting number, under one million, produces the longest chain?
     * 
     * NOTE: Once the chain starts the terms are allowed to go above one million.
     */
    public class Problem014 : ProjectEuler1_to_100SolverBase
    {
        public Problem014(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            var pathLengths = new Dictionary<ulong, ulong>() { [1] = 1 };
            var unknownSequenceNumbers = new Stack<ulong>();
            Console.Write("Calculating distances... [          ]");
            Console.CursorLeft -= 11;

            for (ulong startingPoint = 1; startingPoint < 1_000_000; startingPoint++)
            {
                foreach (var item in Sequences.Collatz(startingPoint))
                {
                    if (!pathLengths.ContainsKey(item))
                        unknownSequenceNumbers.Push(item);
                    else
                    {
                        var rootDistance = pathLengths[item];
                        while (unknownSequenceNumbers.Any())
                        {
                            var reverseItem = unknownSequenceNumbers.Pop();
                            rootDistance++;
                            pathLengths[reverseItem] = rootDistance;
                        }
                        break;
                    }
                }
                if (startingPoint % 100000 == 99999)
                    Console.Write(".");
            }

            var bestDist = pathLengths.Where(p => p.Key < 1_000_000).OrderByDescending(p => p.Value).First();

            IO.LogIfAttached(() => $"Best sequence starting point is {bestDist.Key}, with length {bestDist.Value}");
            return bestDist.Value.ToString();

            /*
            BinaryHeap<int> queue = new BinaryHeap<int>();
            queue.Enqueue(1);

            while (queue.Any())
            {
                var target = queue.Dequeue();
                
                var evenTarget = target * 2;

                if (!pathLengths.ContainsKey(evenTarget))
                {
                    queue.Enqueue(evenTarget);
                    pathLengths[evenTarget] = pathLengths[target] + 1;
                }

                if ((target - 1) % 3 == 0)
                {
                    var oddTarget = (target - 1) / 3;
                    if (oddTarget == 0) continue;

                    if (!pathLengths.ContainsKey(oddTarget))
                    {
                        queue.Enqueue(oddTarget);
                        pathLengths[oddTarget] = pathLengths[target] + 1;
                    }
                }
                if (pathLengths.Keys.Count(p => p < 1_000_000) == 999_999)
                    break;
            }

            return pathLengths.Where(p => p.Key < 1_000_000).Max(p => p.Value).ToString();
            */
        }

    }
}
