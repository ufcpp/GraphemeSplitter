using System.Linq;
using System.Runtime.CompilerServices;

namespace GraphemeSplitter
{
    public partial class Benchmark
    {
        static GraphemeBreakProperty GetByLinearSearch(uint value) => GetByLinearSearch(Items, value);
        static GraphemeBreakProperty GetByBinarySearch(uint value) => GetByBinarySearch(Items, value);

        static GraphemeBreakProperty GetByLinearSearch(PropertyItem[] ranges, uint value)
        {
            var f = ranges.FirstOrDefault(r => r.Min <= value && value <= r.Max);
            if (f.Max == 0) return GraphemeBreakProperty.Other;
            else return f.Property;
        }

        static GraphemeBreakProperty GetByBinarySearch(PropertyItem[] ranges, uint value)
        {
            uint lower = 0;
            uint upper = (uint)(ranges.Length - 1);
            ref var p = ref ranges[0]; // just a little bit faster than by-val

            while (lower <= upper)
            {
                uint middle = (upper + lower) >> 1; // 20% faseter
                ref var r = ref Unsafe.Add(ref p, (int)middle); // a little faster than pointer (avoiding pinning)
                if (value < r.Min) upper = middle - 1;
                else if (value > r.Max) lower = middle + 1;
                else return r.Property;
            }

            return GraphemeBreakProperty.Other;
        }
    }
}
