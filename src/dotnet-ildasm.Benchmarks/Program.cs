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
            ExportEmbeddedResources();

            BenchmarkRunner.Run<AutoIndentationOverhead>();
        }

        private static void ExportEmbeddedResources()
        {
            var assembly = typeof(Program).Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var resourceName in resourceNames)
            {
                var fileName = Path.GetFileName(resourceName);
                Stream resource = assembly.GetManifestResourceStream(resourceName);

                if (!File.Exists(fileName))
                {
                    var fs = File.Create(fileName, (int)resource.Length);
                    var bytesInStream = new byte[resource.Length];
                    resource.Read(bytesInStream, 0, bytesInStream.Length);
                    fs.Write(bytesInStream, 0, bytesInStream.Length);
                }
            }
        }
    }
}