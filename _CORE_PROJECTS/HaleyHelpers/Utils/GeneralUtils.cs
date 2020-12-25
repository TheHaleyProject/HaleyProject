using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Utils
{
    public static class GeneralUtils
    {
        private static Random rand = new Random();
        public static bool getRandomBool()
        {
            if (rand.Next(0, 2) == 0) //Number will be betweewn 0 and 1.
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int getRandomZeroOne()
        {
            return rand.Next(0, 2);
        }
    }
}
