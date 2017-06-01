using BenchmarkDotNet.Running;

namespace dotnet_ildasm.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<FileOutputWriterVSFileStreamOutputWriter>();
        }
    }
}