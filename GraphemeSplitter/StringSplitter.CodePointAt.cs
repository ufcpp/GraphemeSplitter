using System;
using System.Collections.Generic;
using System.Text;

namespace GraphemeSplitter
{
    partial class StringSplitter
    {
        public static (int count, uint cp) CodePointAt(this string str, int index)
        {
            var c = str[index];

            if (char.IsHighSurrogate(c))
            {
                if (index + 1 >= str.Length) return (0, 0);

                var x = (c & 0b00000011_11111111U) + 0b100_0000;
                x <<= 10;
                c = str[index + 1];
                x |= (c & 0b00000011_11111111U);

                return (2, x);
            }
            return (1, c);
        }
    }
}
