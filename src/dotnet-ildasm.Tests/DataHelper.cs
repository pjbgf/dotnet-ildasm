using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace DotNet.Ildasm.Tests
{
    internal static class DataHelper
    {
        internal static readonly Lazy<AssemblyDefinition> SampleAssembly = new Lazy<AssemblyDefinition>(() =>
                Mono.Cecil.AssemblyDefinition.ReadAssembly("dotnet-ildasm.Sample.dll"));
    }
}
