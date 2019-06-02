using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day12Test : TestBase<Day12>
    {
        [Test,
            TestCase("#..#.#..##......###...###\n...##\n..#..\n.#...\n.#.#.\n.#.##\n.##..\n.####\n#.#.#\n#.###\n##.#.\n##.##\n###..\n###.#\n####.", "325")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test]
        public void Part2()
        {
            Assert.Pass("No tests available for part 2");
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("#...#...##..####..##.####.#...#...#.#.#.#......##....#....######.####.##..#..#..##.##..##....#######\n#####\n####.\n###..\n##...\n#..#.\n#.#..\n##.##\n.###.\n.##..\n.#...\n#.#.#\n.#.##\n.##.#\n.#..#\n.#.#.\n..#..\n...##");
            Assert.That("4386", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("5450000001166", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

