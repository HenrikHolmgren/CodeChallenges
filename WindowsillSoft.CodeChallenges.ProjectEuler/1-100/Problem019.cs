using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * You are given the following information, but you may prefer to do some research for yourself.
     * 
     *  - 1 Jan 1900 was a Monday.
     *  - Thirty days has September,
     *    April, June and November.
     *    All the rest have thirty-one,
     *    Saving February alone,
     *    Which has twenty-eight, rain or shine.
     *    And on leap years, twenty-nine.
     *  - A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400.
     *  
     * How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?
     */

    public class Problem019 : ProjectEuler1_to_100SolverBase
    {
        public Problem019(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            //Cheese: 
            //Enumerable.Range(1901, 100)
            //    .Select(year => Enumerable.Range(1, 12)
            //        .Select(month => new DateTime(year, month, 1))
            //        .Count(p => p.DayOfWeek == DayOfWeek.Sunday))
            //    .Sum();

            int SundaysSum = 0;

            var firstDay = 0; //monday;
            for (int year = 1900; year <= 2000; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    firstDay = (firstDay + DaysOfMonth(month, year)) % 7;
                    if (year >= 1901 && firstDay % 7 == 6) //Sundays after Jan 1901
                        SundaysSum++;
                }
            }

            return SundaysSum.ToString();
        }

        private int DaysOfMonth(int month, int year)
        {
            switch (month)
            {
                case 9: // september
                case 4: // april
                case 6: // june
                case 11: // november
                    return 30; //thirty days hath

                case 2: // february alone
                    if (!IsLeapYear(year))
                        return 28; // hath twenty eight 
                    else
                        return 29; // except when it fucks up everything.

                default: //All the rest
                    return 31; //hath thirty one
            }
        }

        private bool IsLeapYear(int year)
        {
            var leap = year % 4 == 0;
            leap &= year % 100 != 0;
            leap |= year % 400 == 0;
            return leap;
        }
    }

}
