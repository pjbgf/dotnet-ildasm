using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using DotNet.Ildasm;
using DotNet.Ildasm.Adapters;
using Mono.Cecil;

namespace dotnet_ildasm.Benchmarks
{
    public class MultipleRuntimes : ManualConfig
    {
        public MultipleRuntimes()
        {
            Add(Job.Default.With(CsProjCoreToolchain.NetCoreApp21));
            Add(Job.Default.With(CsProjClassicNetToolchain.Net461));
        }
    }
    
    [Config(typeof(MultipleRuntimes))]
    public class AutoIndentationOverhead
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
            Mono.Cecil.AssemblyDefinition.ReadAssembly(
                typeof(Program).Assembly.GetManifestResourceStream(
                    "dotnet-ildasm.Benchmarks.Sample.dotnet-ildasm.Sample.exe")));

        private static readonly MethodDefinition MethodDefinition;

        static AutoIndentationOverhead()
        {
            var type = SampleAssembly.Value.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            MethodDefinition = type.Methods.FirstOrDefault(x => x.Name == "UsingTryCatch");
        }

        [Benchmark]
        public void NoIndentation()
        {
            using (var fileStreamOutputWriter = new FileStreamOutputWriter(Path.GetTempFileName()))
            {
                MethodDefinition.WriteILBody(fileStreamOutputWriter);
            }
        }

        [Benchmark]
        public void AutoIndentation()
        {
            using (var outputWriter = new FileStreamOutputWriter(Path.GetTempFileName()))
            using (var autoIndentOutputWriter = new AutoIndentOutputWriter(outputWriter))
            {
                MethodDefinition.WriteILBody(autoIndentOutputWriter);
            }
        }
    }
}