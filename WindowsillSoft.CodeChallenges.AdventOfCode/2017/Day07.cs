using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day07 : AdventOfCode2017SolverBase
    {
        private RecursiveTreeNode[] _treeRoots;

        public Day07(IIOProvider provider) : base(provider) => _treeRoots = new RecursiveTreeNode[0];

        public override string Name => "Day 7: Recursive Circus";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"(?'node'[a-zA-Z]+) \((?'value'\d*)\)( -> (?'children'.*))?", RegexOptions.Compiled);

            var nodes = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => matcher.Match(p))
                .Select(p => new
                {
                    name = p.Groups["node"].Value,
                    value = Int32.Parse(p.Groups["value"].Value),
                    children = p.Groups["children"].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(q => q.Trim())
                        .ToArray()
                }).ToList();

            var treeNodes = nodes.Select(p => new RecursiveTreeNode(p.name, p.value))
                .ToDictionary(p => p.Name);

            foreach (var node in nodes)
            {
                var treeNode = treeNodes[node.name];
                var children = node.children.Select(p => treeNodes[p]);
                foreach (var child in children)
                    treeNode.AddChild(child);
            }

            _treeRoots = treeNodes.Values.Where(p => p.Parent == null).ToArray();
        }

        public override string ExecutePart1() => _treeRoots.Single().Name;


        public override string ExecutePart2()
        {
            var root = _treeRoots.Single();
            return GetBalanceChange(root).ToString();
        }

        private int? GetBalanceChange(RecursiveTreeNode root)
        {
            if (!root.Children.Any())
                return null;

            var values = root.Children.GroupBy(p => p.BalanceValue);
            if (values.Count() == 1)
                return null;

            if (values.Count(p => p.Count() == 1) == 1 && values.Any(p => p.Count() > 1))
            {
                var misfitGroup = values.First(p => p.Count() == 1).Single();
                var targetNumber = values.First(p => p.Count() != 1).First().BalanceValue;
                return GetBalanceChange(misfitGroup) ?? targetNumber - misfitGroup.BalanceValue + misfitGroup.Value;
            }

            var childRebalancing = root.Children.Select(p => GetBalanceChange(p)).ToList();
            return childRebalancing.First(p => p != null);
        }

        public class RecursiveTreeNode
        {
            public string Name { get; }
            public int Value { get; }
            public List<RecursiveTreeNode> Children { get; }
            public RecursiveTreeNode? Parent { get; private set; }

            public RecursiveTreeNode(string name, int value, IEnumerable<RecursiveTreeNode> children)
            {
                Name = name;
                Value = value;
                Children = children.ToList();
                _balanceValue = new Lazy<int>(GetBalanceValue);
            }
            public RecursiveTreeNode(string name, int value) : this(name, value, Enumerable.Empty<RecursiveTreeNode>()) { }

            public void AddChild(RecursiveTreeNode child)
            {
                Children.Add(child);
                child.Parent = this;
            }
            public Lazy<int> _balanceValue;

            private int GetBalanceValue() => Value + Children.Sum(p => p.BalanceValue);

            public int BalanceValue => _balanceValue.Value;
        }
    }
}
