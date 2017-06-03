using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Tests.Internal
{
    internal static class DataHelper
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
                Mono.Cecil.AssemblyDefinition.ReadAssembly("dotnet-ildasm.Sample.dll"));
    }
}
