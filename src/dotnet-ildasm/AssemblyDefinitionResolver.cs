using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class AssemblyDefinitionResolver : IAssemblyDefinitionResolver
    {
        public AssemblyDefinition Resolve(string assemblyPath)
        {
            return Mono.Cecil.AssemblyDefinition.ReadAssembly(assemblyPath);
        }
    }
}