using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2019.Common
{
    class IntMachineTests
    {
        [Test,
            TestCase(new long[] { 1, 0, 0, 0, 99 }, 2),
            TestCase(new long[] { 2, 3, 0, 3, 99 }, 2),
            TestCase(new long[] { 2, 4, 4, 5, 99, 0 }, 2),
            TestCase(new long[] { 1, 0, 0, 0, 99 }, 2),
            TestCase(new long[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, 30)]
        public void Executes_Day02_test_programs(long[] program, int result)
        {
            var machine = new IntCodeMachine(program);
            machine.Run();

            Assert.That(machine.Peek(0), Is.EqualTo(result));
        }

        [Test]
        public void Allows_input_output()
        {
            var program = new long[] { 3, 0, 4, 0, 99 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(42);
            Assert.That(machine.LastOutput, Is.EqualTo(42));
        }

        [Test]
        public void Supports_immediate_mode()
        {
            var program = new long[] { 1002, 4, 3, 4, 33 };
            var machine = new IntCodeMachine(program);
            machine.Run();

            Assert.That(machine.Peek(4), Is.EqualTo(99));
        }

        [Test]
        public void Allows_negative_parameters()
        {
            var program = new long[] { 1101, 100, -1, 4, 0 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            Assert.That(machine.Peek(4), Is.EqualTo(99));
        }

        [Test, TestCase(7, 0), TestCase(8, 1), TestCase(9, 0)]
        public void Conditionals_equality_position_mode(int input, int result)
        {
            var program = new long[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(7, 0), TestCase(8, 1), TestCase(9, 0)]
        public void Conditionals_equality_immediate_mode(int input, int result)
        {
            var program = new long[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(7, 1), TestCase(8, 0), TestCase(9, 0)]
        public void Conditionals_lessThan_position_mode(int input, int result)
        {
            var program = new long[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(7, 1), TestCase(8, 0), TestCase(9, 0)]
        public void Conditionals_lessThan_immediate_mode(int input, int result)
        {
            var program = new long[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(1, 1), TestCase(-1, 1), TestCase(0, 0), TestCase(99, 1)]
        public void Conditional_jump_position_mode(int input, int result)
        {
            var program = new long[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(1, 1), TestCase(-1, 1), TestCase(0, 0), TestCase(99, 1)]
        public void Conditional_jump_immediate_mode(int input, int result)
        {
            var program = new long[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test, TestCase(-100, 999), TestCase(0, 999), TestCase(7, 999), TestCase(8, 1000), TestCase(9, 1001), TestCase(99, 1001)]
        public void Compound_conditionals_mixed_mode(int input, int result)
        {
            var program = new long[] {
                3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(input);

            Assert.That(machine.LastOutput, Is.EqualTo(result));
        }

        [Test]
        public void Quine()
        {
            var program = new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            var machine = new IntCodeMachine(program);
            int pc = 0;
            machine.Run();
            while (machine.CurrentState is IntCodeMachine.OutputAvailableState)
            {
                Assert.That(program[pc], Is.EqualTo(machine.LastOutput));
                pc++;
                machine.AcceptOutputAndContinue();
            }
            Assert.That(pc, Is.EqualTo(program.Length));
        }

        [Test]
        public void SupportsLargeNumbers_1()
        {
            var program = new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            Assert.That(machine.LastOutput, Is.GreaterThanOrEqualTo(1_000_000_000_000_000L));
            Assert.That(machine.LastOutput, Is.LessThanOrEqualTo(10_000_000_000_000_000L));
        }
        [Test]
        public void SupportsLargeNumbers_2()
        {
            var program = new long[] { 104, 1125899906842624, 99 };
            var machine = new IntCodeMachine(program);
            machine.Run();
            Assert.That(machine.LastOutput, Is.EqualTo(program[1]));
        }
    }
}
