using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using DotNet.Ildasm;
using DotNet.Ildasm.Adapters;
using Mono.Cecil;

namespace dotnet_ildasm.Benchmarks
{
    [CoreJob][ClrJob]
    public class AutoIndentationOverhead
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
            Mono.Cecil.AssemblyDefinition.ReadAssembly("c:\\git\\dotnet-ildasm\\src\\dotnet-ildasm.Sample\\bin\\Debug\\net45\\dotnet-ildasm.Sample.exe"));

        private static MethodDefinition _methodDefinition;
        private static readonly string TargetILFile = "C:\\git\\dotnet-ildasm\\dotnet-ildasm.Sample.il";

        static AutoIndentationOverhead()
        {
            var type = SampleAssembly.Value.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            _methodDefinition = type.Methods.FirstOrDefault(x => x.Name == "UsingTryCatch");
        }

        [Benchmark]
        public void NoIndentation()
        {
            using (var fileStreamOutputWriter = new FileStreamOutputWriter(TargetILFile))
            {
                _methodDefinition.WriteILBody(fileStreamOutputWriter);
            }
        }

        [Benchmark]
        public void AutoIndentation()
        {
            using (var outputWriter = new FileStreamOutputWriter(TargetILFile))
            using (var autoIndentOutputWriter = new AutoIndentOutputWriter(outputWriter))
            {
                _methodDefinition.WriteILBody(autoIndentOutputWriter);
            }
        }
    }
}