using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Numerics;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
    * Starting in the top left corner of a 2×2 grid, and only being able to move to the right and down, there are exactly 6 routes to the bottom right corner.
    * 
    * How many such routes are there through a 20×20 grid?
    */
    public class Problem015 : ProjectEuler1_to_100SolverBase
    {
        public Problem015(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            ulong sideLength = 20;
            return WsMath.Choose(2 * sideLength, sideLength).ToString();
        }
    }
}
