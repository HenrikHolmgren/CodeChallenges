using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day07 : AdventOfCode2018SolverBase
    {
        private int _taskTime = 60;
        private int _workerCount = 5;
        private Edge[] _edges = new Edge[0];

        public Day07(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 7: The Sum of Its Parts";

        public override void Initialize(string input)
        {
            string? workerCount;
            do
            {
                workerCount = IO.RequestInput("Worker count?");
            } while (workerCount == default || !Int32.TryParse(workerCount, out _workerCount));


            string? additionalTimePrTask;
            do
            {
                additionalTimePrTask = IO.RequestInput("Additional time pr. task?");
            } while (additionalTimePrTask == default || !Int32.TryParse(additionalTimePrTask, out _taskTime));


            var matcher = new Regex(@"Step (?'from'.) must be finished before step (?'to'.) can begin.");

            _edges = ReadAndSplitInput<string>(input)
                .Select(p => matcher.Match(p))
                .Select(p => new Edge(p.Groups["from"].Value[0], p.Groups["to"].Value[0]))
                .ToArray();
        }

        public override string ExecutePart1()
        {
            var result = new StringBuilder();
            var nodes = BuildGraph(_edges);

            var fringe = nodes.Where(p => !p.Parents.Any()).ToList();

            while (fringe.Any())
            {
                var action = fringe.OrderBy(p => p.Value).First();
                fringe.Remove(action);
                result.Append(action.Value);
                action.Children.ForEach(p => p.Parents.Remove(action));
                fringe.AddRange(action.Children.Where(p => p.Parents.Count == 0));
            }
            return result.ToString();
        }

        public override string ExecutePart2()
        {
            var nodes = BuildGraph(_edges);
            var fringe = nodes.Where(p => !p.Parents.Any()).ToList();

            var currentTasks = new List<(Node task, int finishTime)>();

            var currentTime = 0;

            while (fringe.Any())
            {
                var action = fringe.OrderBy(p => p.Value).First();
                var delayTime = action.Value - 'A' + 1 + _taskTime;
                fringe.Remove(action);
                if (currentTasks.Count() == _workerCount)
                {
                    var finishedTask = currentTasks.OrderBy(p => p.finishTime).First();
                    currentTime = finishedTask.finishTime;
                    currentTasks.Remove(finishedTask);
                    finishedTask.task.Children.ForEach(p => p.Parents.Remove(finishedTask.task));
                    fringe.AddRange(finishedTask.task.Children.Where(p => p.Parents.Count == 0));
                }

                currentTasks.Add((action, delayTime + currentTime));

                while (!fringe.Any() && currentTasks.Any())
                {
                    currentTime = currentTasks.Min(p => p.finishTime);

                    var finishedTask = currentTasks.Where(p => p.finishTime <= currentTime).ToList();
                    foreach (var task in finishedTask)
                    {
                        currentTasks.Remove(task);
                        task.task.Children.ForEach(p => p.Parents.Remove(task.task));
                        fringe.AddRange(task.task.Children.Where(p => p.Parents.Count == 0));
                    }
                }
            }
            return currentTime.ToString();
        }

        private static List<Node> BuildGraph(Edge[] input)
        {
            var nodes = input.Select(p => p.From).Concat(input.Select(p => p.To))
                            .Distinct()
                            .ToDictionary(p => p, p => new Node { Value = p });

            foreach (var edge in input)
            {
                nodes[edge.From].Children.Add(nodes[edge.To]);
            }

            foreach (var node in nodes)
            {
                node.Value.Children.ForEach(p => p.Parents.Add(node.Value));
            }

            return nodes.Values.ToList();
        }

        public struct Edge
        {
            public char From { get; set; }
            public char To { get; set; }
            public Edge(char from, char to)
                => (From, To) = (from, to);
        }

        public class Node
        {
            public List<Node> Parents { get; } = new List<Node>();
            public List<Node> Children { get; } = new List<Node>();
            public char Value { get; set; }
        }
    }
}