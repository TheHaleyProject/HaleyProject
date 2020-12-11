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
            if (character == '\u0000')
            {
                return source.PadLeft(pad_left).PadRight(length);
            }
            else
            {
                return source.PadLeft(pad_left, character).PadRight(length, character);
            }
        }
        public static bool isBase64(this string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length % 4 != 0
               || input.Contains(" ") || input.Contains("\t") || input.Contains("\r") || input.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string toNumber(this string input)
        {
            string numbered_key = string.Empty;
            foreach (var _char in input)
            {
                int index = (int)_char % 32;
                numbered_key += index;
            }
            return numbered_key;
        }
    }
}
