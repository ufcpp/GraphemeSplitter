using BenchmarkDotNet.Running;

namespace GraphemeBreakPropertyCodeGeneratorTest
{
    /// <summary>
    ///          Method |         Mean |     Error |    StdDev |
    /// --------------- |-------------:|----------:|----------:|
    ///  ByLinearSearch | 1,061.146 ms | 1.3965 ms | 1.0903 ms |
    ///  ByBinarySearch |     4.642 ms | 0.0468 ms | 0.0415 ms |
    ///        BySwitch |     3.825 ms | 0.0473 ms | 0.0442 ms |
    ///    BySwitchWhen |   115.310 ms | 2.7970 ms | 2.9928 ms |
    ///            ByIf |   142.658 ms | 1.3744 ms | 1.1477 ms |
    ///      ByBinaryIf |     2.502 ms | 0.0147 ms | 0.0137 ms |
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
