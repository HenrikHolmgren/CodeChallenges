using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day6;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class Day6Input
    {
        //First line: distance threshold = 32
        public const string Test1Input = @"32
1, 1
1, 6
8, 3
3, 4
5, 5
8, 9";
        public const string Part1Test1Result = "17";
        public const string Part2Test1Result = "16";

        [FullRunInput(typeof(Day6Solver))]
        public const string FullRunInput = @"10000
355, 246
259, 215
166, 247
280, 341
54, 91
314, 209
256, 272
149, 313
217, 274
299, 144
355, 73
70, 101
266, 327
51, 228
274, 123
342, 232
97, 100
58, 157
130, 185
135, 322
306, 165
335, 84
268, 234
173, 255
316, 75
79, 196
152, 71
205, 261
275, 342
164, 95
343, 147
83, 268
74, 175
225, 130
354, 278
123, 206
166, 166
155, 176
282, 238
107, 295
82, 92
325, 299
87, 287
90, 246
159, 174
295, 298
260, 120
203, 160
72, 197
182, 296";
        public const string Part1FullRunOutput = "3238";
        public const string Part2FullRunOutput = "45046";
    }
}
