using BenchmarkDotNet.Running;

namespace GraphemeBreakPropertyCodeGeneratorTest
{
    /// <summary>
    ///          Method |         Mean |     Error |    StdDev |
    /// --------------- |-------------:|----------:|----------:|
    ///  ByLinearSearch | 1,061.146 ms | 1.3965 ms | 1.0903 ms |
    ///  ByBinarySearch |     3.604 ms | 0.0115 ms | 0.0102 ms |
    ///        BySwitch |     3.351 ms | 0.0181 ms | 0.0169 ms |
    ///            ByIf |    89.596 ms | 0.3865 ms | 0.3426 ms |
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GraphemeSplitter.Benchmark.Test();
            BenchmarkRunner.Run<GraphemeSplitter.Benchmark>();
        }
    }
}
