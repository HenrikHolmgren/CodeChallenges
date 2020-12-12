using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2020;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2020
{
    public class Day10Test : TestBase<Day10>
    {
        [Test,
            TestCase("16\n10\n15\n5\n1\n11\n7\n19\n6\n12\n4", "35"),
            TestCase("28\n33\n18\n42\n31\n14\n46\n20\n48\n47\n24\n23\n49\n45\n19\n38\n39\n11\n1\n32\n25\n35\n8\n17\n7\n9\n4\n2\n34\n10\n3", "220")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test, 
            TestCase("16\n10\n15\n5\n1\n11\n7\n19\n6\n12\n4", "8"),
            TestCase("28\n33\n18\n42\n31\n14\n46\n20\n48\n47\n24\n23\n49\n45\n19\n38\n39\n11\n1\n32\n25\n35\n8\n17\n7\n9\n4\n2\n34\n10\n3", "19208")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }
        [Test]
        public void FullRun()
        {
            var solver = GetSolver("79\n142\n139\n33\n56\n133\n138\n61\n125\n88\n158\n123\n65\n69\n105\n6\n81\n31\n60\n70\n159\n114\n71\n15\n13\n72\n118\n14\n9\n93\n162\n140\n165\n1\n80\n148\n32\n53\n102\n5\n68\n101\n111\n85\n45\n25\n16\n59\n131\n23\n91\n92\n115\n103\n166\n22\n145\n161\n108\n155\n135\n55\n86\n34\n37\n78\n28\n75\n7\n104\n121\n24\n153\n167\n95\n87\n94\n134\n154\n84\n151\n124\n62\n49\n38\n39\n54\n109\n128\n19\n2\n98\n122\n132\n141\n168\n8\n160\n50\n42\n46\n110\n12\n152");
            Assert.That(solver.ExecutePart1(), Is.EqualTo("2376"));
            Assert.That(solver.ExecutePart2(), Is.EqualTo("129586085429248"));
        }
    }
}