using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Utils
{
    public static class StringHelpers
    {
        public static string padCenter(this string source, int length, char character = '\u0000')
        {
            
            int space_available = length - source.Length;
            int pad_left = (space_available / 2) + source.Length; //Amount to pad left.
            if (character == null)
            {
                return source.PadLeft(pad_left).PadRight(length);
            }
            else
            {
                return source.PadLeft(pad_left, character).PadRight(length, character);
            }
        }
    }
}
