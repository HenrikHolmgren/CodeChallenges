using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2019
{
    public class Day01Test : TestBase<Day01>
    {
        [Test,
            TestCase("12", "2"),
            TestCase("14", "2"),
            TestCase("1969", "654"),
            TestCase("100756", "33583"),
            TestCase("12\n14\n1969\n100756", "34241")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("14", "2"),
            TestCase("1969", "966"),
            TestCase("100756", "50346")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("70904\n66770\n118678\n58460\n128566\n60820\n107153\n113196\n52413\n118335\n96711\n88120\n129744\n64049\n90586\n54466\n55693\n102407\n148273\n110281\n111814\n60951\n102879\n135253\n130081\n86645\n72934\n147097\n74578\n124073\n100003\n103314\n86468\n84557\n94232\n120012\n64372\n143081\n96664\n148076\n147357\n139897\n113139\n143022\n144298\n81293\n53679\n139311\n107156\n121730\n132519\n132666\n80464\n111118\n76734\n139023\n111287\n126811\n130539\n129173\n67549\n102058\n72673\n91194\n64753\n59488\n126300\n94407\n126813\n60028\n95129\n79270\n123465\n60966\n111920\n76549\n110195\n119975\n112557\n129676\n104941\n89583\n121895\n108901\n135247\n75129\n148646\n131128\n78931\n111637\n72752\n140761\n57387\n85684\n77596\n134159\n63031\n148361\n133856\n82022");
            Assert.That("3423279", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("5132018", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

