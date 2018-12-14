using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day7
{
    public class Day7Solver : IProblemSolver
    {
        const int WORK_CONSTANT = 60;
        const int WORKER_COUNT = 5;

        public string Description => "Day 7";

        public int SortOrder => 7;

        public void Solve()
        {
            //var input = new[] {
            //    new Edge('C', 'A'),
            //    new Edge('C', 'F'),
            //    new Edge('A', 'B'),
            //    new Edge('A', 'D'),
            //    new Edge('B', 'E'),
            //    new Edge('D', 'E'),
            //    new Edge('F', 'E'),
            //};

            var matcher = new Regex(@"Step (?'from'.) must be finished before step (?'to'.) can begin.");
            var input = File.ReadAllLines("Day7/Day7Input.txt")
                .Select(p => matcher.Match(p))
                .Select(p => new Edge(p.Groups["from"].Value[0], p.Groups["to"].Value[0]))
                .ToArray();

            Part1(input);

            Part2(input);

        }

        private static void Part2(Edge[] input)
        {
            var nodes = BuildGraph(input);
            var fringe = nodes.Where(p => !p.Parents.Any()).ToList();

            var currentTasks = new List<(Node task, int finishTime)>();

            var currentTime = 0;

            while (fringe.Any())
            {
                var action = fringe.OrderBy(p => p.Value).First();
                var delayTime = action.Value - 'A' + 1 + WORK_CONSTANT;
                fringe.Remove(action);
                if (currentTasks.Count() == WORKER_COUNT)
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
            Console.WriteLine($"All tasks finished in {currentTime}");
        }

        private static void Part1(Edge[] input)
        {
            var nodes = BuildGraph(input);

            var fringe = nodes.Where(p => !p.Parents.Any()).ToList();

            while (fringe.Any())
            {
                var action = fringe.OrderBy(p => p.Value).First();
                fringe.Remove(action);
                Console.Write(action.Value);
                action.Children.ForEach(p => p.Parents.Remove(action));
                fringe.AddRange(action.Children.Where(p => p.Parents.Count == 0));
            }

            Console.WriteLine();
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
    }

    struct Edge
    {
        public char From { get; set; }
        public char To { get; set; }
        public Edge(char from, char to)
            => (From, To) = (from, to);
    }

    class Node
    {
        public List<Node> Parents { get; } = new List<Node>();
        public List<Node> Children { get; } = new List<Node>();
        public char Value { get; set; }
    }
}
