using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Configs;
using DotNet.Ildasm;
using DotNet.Ildasm.Adapters;
using Mono.Cecil;

namespace dotnet_ildasm.Benchmarks
{
    [CoreJob]
    public class AutoIndentationOverhead
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
            Mono.Cecil.AssemblyDefinition.ReadAssembly("..\\..\\..\\..\\dotnet-ildasm.Benchmarks.Sample.dotnet-ildasm.Sample.dll"));

        private static readonly MethodDefinition MethodDefinition;

        static AutoIndentationOverhead()
        {
            var type = SampleAssembly.Value.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            MethodDefinition = type.Methods.FirstOrDefault(x => x.Name == "UsingTryCatch");
        }

        [Benchmark]
        public void NoIndentation()
        {
            using (var fileStreamOutputWriter = new FileStreamOutputWriter(Path.GetRandomFileName()))
            {
                MethodDefinition.WriteILBody(fileStreamOutputWriter);
            }
        }

        [Benchmark]
        public void AutoIndentation()
        {
            using (var outputWriter = new FileStreamOutputWriter(Path.GetRandomFileName()))
            using (var autoIndentOutputWriter = new AutoIndentOutputWriter(outputWriter))
            {
                MethodDefinition.WriteILBody(autoIndentOutputWriter);
            }
        }
    }
}