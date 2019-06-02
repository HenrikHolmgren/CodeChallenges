using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Geometry;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day20 : AdventOfCode2017SolverBase
    {
        private List<Particle> _particles;

        public Day20(IIOProvider provider) : base(provider) => _particles = new List<Particle>();

        public override string Name => "Day 20: Particle Swarm";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>", RegexOptions.Compiled);
            _particles = new List<Particle>();

            foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var match = matcher.Match(line);
                var pos = new ManhattanPointNInt(match.Groups.Values.Skip(1).Take(3).Select(p => Int32.Parse(p.Value)).ToArray());
                var vel = new ManhattanPointNInt(match.Groups.Values.Skip(4).Take(3).Select(p => Int32.Parse(p.Value)).ToArray());
                var acc = new ManhattanPointNInt(match.Groups.Values.Skip(7).Take(3).Select(p => Int32.Parse(p.Value)).ToArray());

                _particles.Add(new Particle
                {
                    P = pos,
                    V = vel,
                    A = acc
                });
            }
        }

        public override string ExecutePart1()
        {
            //'In the long run' ~= infinite time.
            //So lowest acceleration will always win out, tie break on lowest initial speed, then on lowest initial position.
            var winner = _particles.OrderBy(p => p.A.Length)
                .ThenBy(p => p.V.Length)
                .ThenBy(p => p.P.Length)
                .First();

            return _particles.IndexOf(winner).ToString();
        }

        public override string ExecutePart2()
        {
            var swarm = _particles.Select(p => p.Clone()).ToList();

            //Todo: Proper stop condition - good candidate is to stop whenever all particle pairs have larger distances than 
            //they did in the last time step.
            //Final collisions actually happen at time T=39
            for (var t = 0; t < 1000; t++)
            {
                swarm.ForEach(p => p.V += p.A);
                swarm.ForEach(p => p.P += p.V);

                var newDist = new List<int>();

                for (var i = 0; i < swarm.Count; i++)
                {
                    var collisions = swarm.Where(p => p.P == swarm[i].P).ToList();
                    if (collisions.Count > 1)
                        swarm.RemoveAll(p => collisions.Contains(p));
                }
            }

            return swarm.Count.ToString();
        }

        private class Particle
        {
            public ManhattanPointNInt P { get; set; }
            public ManhattanPointNInt V { get; set; }
            public ManhattanPointNInt A { get; set; }

            public Particle Clone()
                => new Particle { P = P, V = V, A = A };

            public override string ToString()
                => $"<p={P}, v={V}, a={A}>";
        }
    }
}
