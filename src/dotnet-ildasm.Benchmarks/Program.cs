using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.IO;
using System.Reflection;

namespace dotnet_ildasm.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AutoIndentationOverhead>();
        }
    }
}