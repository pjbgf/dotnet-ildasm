using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using DotNet.Ildasm;
using DotNet.Ildasm.Adapters;
using Mono.Cecil;

namespace dotnet_ildasm.Benchmarks
{
    [CoreJob][ClrJob]
    public class FileOutputWriterVSFileStreamOutputWriter
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
            Mono.Cecil.AssemblyDefinition.ReadAssembly("c:\\git\\dotnet-ildasm\\src\\dotnet-ildasm.Sample\\bin\\Debug\\net45\\dotnet-ildasm.Sample.exe"));

        private static MethodDefinition _methodDefinition;
        private static readonly IndentationProvider IndentationProvider = new IndentationProvider();
        private static readonly string TargetILFile = "C:\\git\\dotnet-ildasm\\dotnet-ildasm.Sample.il";

        static FileOutputWriterVSFileStreamOutputWriter()
        {
            var type = SampleAssembly.Value.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            _methodDefinition = type.Methods.FirstOrDefault(x => x.Name == "UsingTryCatch");
        }

        [Benchmark]
        public void FileStreamOutputWriter()
        {
            using (var fileStreamOutputWriter = new FileStreamOutputWriter(IndentationProvider, TargetILFile))
            {
                var processor = new MethodProcessor(fileStreamOutputWriter);
                processor.WriteBody(_methodDefinition);
            }
        }

        [Benchmark]
        public void FileOutputWriter()
        {
            using (var fileOutputWriter = new FileOutputWriter(IndentationProvider, TargetILFile))
            {
                var processor = new MethodProcessor(fileOutputWriter);
                processor.WriteBody(_methodDefinition);
            }
        }
    }
}