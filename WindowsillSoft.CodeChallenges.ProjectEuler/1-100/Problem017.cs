using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    public class Problem017 : ProjectEuler1_to_100SolverBase
    {
        public Problem017(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            return Enumerable.Range(1, 1000)
                .Select(p => ToEnglish(p)
                .Where(q => Char.IsLetter(q)).Count())
                .Sum()
                .ToString();
        }

        public string ToEnglish(int number)
        {
            string result = "";
            if (number % 100 > 9 && number % 100 < 20)
            {
                switch (number % 100)
                {
                    case 10:
                        result = "ten";
                        break;
                    case 11:
                        result = "eleven";
                        break;
                    case 12:
                        result = "twelve";
                        break;
                    case 13:
                        result = "thirteen";
                        break;
                    case 14:
                        result = "fourteen";
                        break;
                    case 15:
                        result = "fifteen";
                        break;
                    case 16:
                        result = "sixteen";
                        break;
                    case 17:
                        result = "seventeen";
                        break;
                    case 18:
                        result = "eighteen";
                        break;
                    case 19:
                        result = "nineteen";
                        break;
                }
            }
            else
            {
                string ones = ToOrdinal(number % 10);
                string tens = "";

                switch ((number / 10) % 10)
                {
                    case 2:
                        tens = "twenty";
                        break;
                    case 3:
                        tens = "thirty";
                        break;
                    case 4:
                        tens = "forty";
                        break;
                    case 5:
                        tens = "fifty";
                        break;
                    case 6:
                        tens = "sixty";
                        break;
                    case 7:
                        tens = "seventy";
                        break;
                    case 8:
                        tens = "eighty";
                        break;
                    case 9:
                        tens = "ninety";
                        break;
                }

                if (!string.IsNullOrEmpty(tens) && !string.IsNullOrEmpty(ones))
                    result = $"{tens}-{ones}";
                else
                    result = tens + ones;
            }

            if (number >= 100)
            {
                if (!string.IsNullOrEmpty(result))
                    result = $"and {result}";

                var hundreds = ToOrdinal(number / 100 % 10);
                if (!string.IsNullOrEmpty(hundreds))
                    result = $"{hundreds} hundred {result}".Trim();

                if (number >= 1000)
                {
                    var thousands = ToOrdinal(number / 1000 % 10);
                    if (!string.IsNullOrEmpty(thousands))
                        result = $"{thousands} thousand {result}".Trim();
                }
            }
            return result;
        }

        private string ToOrdinal(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    return "";
            }
        }
    }

}
