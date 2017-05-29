using System;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class AssemblyDefinitionResolver : IAssemblyDefinitionResolver
    {
        public AssemblyDefinition Resolve(string assemblyPath)
        {
            try
            {
                return Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);
            }
            catch
            {
                return null;
            }
        }
    }
}