using BenchmarkDotNet.Attributes;
using System;
using Xunit;

namespace GraphemeSplitter
{
    public partial class Benchmark
    {

        const int Loops = 100000;
        const int Mask = 0x1FFFFF;

        public static void Test()
        {
            var r = new Random();

            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() & Mask;

                Assert.Equal(GetBySwitch(v), GetByIf(v));
                Assert.Equal(GetBySwitch(v), GetByBinaryIf(v));
                Assert.Equal(GetBySwitch(v), GetByLinearSearch(v));
                Assert.Equal(GetBySwitch(v), GetByBinarySearch(v));
            }
        }

        // too slow not to be thought
        // ×100 slower than others
        //[Benchmark]
        public void ByLinearSearch()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetByLinearSearch(v);
            }
        }

        [Benchmark]
        public void ByBinarySearch()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetByBinarySearch(v);
            }
        }

        [Benchmark]
        public void BySwitch()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetBySwitch(v);
            }
        }

        [Benchmark]
        public void BySwitchWhen()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetBySwitchWhen(v);
            }
        }

        // ×10 slower than ByBinarySearch and BySwitch
        [Benchmark]
        public void ByIf()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetByIf(v);
            }
        }

        [Benchmark]
        public void ByBinaryIf()
        {
            var r = new Random(1);
            for (int i = 0; i < Loops; i++)
            {
                var v = (uint)r.Next() % Mask;
                GetByBinaryIf(v);
            }
        }
    }
}
