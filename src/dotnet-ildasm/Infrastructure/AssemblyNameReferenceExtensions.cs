using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class AssemblyNameReferenceExtensions
    {
        public static void WriteIL(this AssemblyNameReference reference, IOutputWriter writer)
        {
            writer.WriteLine($".assembly extern {reference.Name}");
            writer.WriteLine("{");
            writer.WriteLine($".publickeytoken = ( {reference.PublicKeyToken.ToHexadecimal()} )");
            writer.WriteLine($".ver {reference.Version.Major}:{reference.Version.Minor}:{reference.Version.Revision}:{reference.Version.Build}");
            writer.WriteLine("}");
        }
    }
}
