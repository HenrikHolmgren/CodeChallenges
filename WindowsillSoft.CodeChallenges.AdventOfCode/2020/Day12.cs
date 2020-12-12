using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day12 : AdventOfCode2020SolverBase
    {
        protected record Instruction(char Command, int Length);

        protected record WaypointBoat(int East, int South, int WaypointEast, int WaypointSouth)
        {
            public WaypointBoat ApplyInstruction(Instruction i)
            {
                switch (i.Command)
                {
                    case 'F': return this with { South = this.South + i.Length * WaypointSouth, East = this.East + i.Length * WaypointEast };
                    case 'N': return this with { WaypointSouth = this.WaypointSouth - i.Length };
                    case 'S': return this with { WaypointSouth = this.WaypointSouth + i.Length };
                    case 'E': return this with { WaypointEast = this.WaypointEast + i.Length };
                    case 'W': return this with { WaypointEast = this.WaypointEast - i.Length };
                    case 'L':
                        switch (i.Length)
                        {
                            case 0: return this;
                            case 90: return this with { WaypointEast = this.WaypointSouth, WaypointSouth = -this.WaypointEast };
                            case 180: return this with { WaypointEast = -this.WaypointEast, WaypointSouth = -this.WaypointSouth };
                            case 270: return this with { WaypointEast = -this.WaypointSouth, WaypointSouth = this.WaypointEast };
                            default: throw new InvalidOperationException("Drunk captain!");
                        }
                    case 'R': return ApplyInstruction(new Instruction('L', 360 - i.Length));
                    default: throw new InvalidOperationException($"Unknown instruction {i.Command}!");
                }
            }
        }
        protected record SimpleBoat(int Heading, int East, int South)
        {
            public SimpleBoat ApplyInstruction(Instruction i)
            {
                switch (i.Command)
                {
                    case 'F':
                        switch (Heading)
                        {
                            case 0: return this with { East = this.East + i.Length };
                            case 90: return this with { South = this.South + i.Length };
                            case 180: return this with { East = this.East - i.Length };
                            case 270: return this with { South = this.South - i.Length };
                            default: throw new InvalidOperationException("Drunk Captain!");
                        }
                    case 'N': return this with { South = this.South - i.Length };
                    case 'S': return this with { South = this.South + i.Length };
                    case 'E': return this with { East = this.East + i.Length };
                    case 'W': return this with { East = this.East - i.Length };

                    case 'L': return this with { Heading = (this.Heading - i.Length + 3600) % 360 };
                    case 'R': return this with { Heading = (this.Heading + i.Length + 3600) % 360 };
                    default: throw new InvalidOperationException($"Unknown instruction {i.Command}!");
                }
            }
        }

        private Instruction[] _instructions = new Instruction[0];


        public Day12(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 12: Rain Risk";
        public override string ExecutePart1()
        {           
            var boat = new SimpleBoat(0, 0, 0);
            foreach (var instruction in _instructions)
                boat = boat.ApplyInstruction(instruction);
            return (Math.Abs(boat.East) + Math.Abs(boat.South)).ToString();
        }

        public override string ExecutePart2()
        {
            var boat = new WaypointBoat(0, 0, 10, -1);
            foreach (var instruction in _instructions)
                boat = boat.ApplyInstruction(instruction);
            return (Math.Abs(boat.East) + Math.Abs(boat.South)).ToString();
        }
        public override void Initialize(string input) =>
            _instructions = ReadAndSplitInput<Instruction>(input, p => new Instruction(p[0], int.Parse(p[1..])))
            .ToArray();
    }
}
