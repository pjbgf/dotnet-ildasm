using Mono.Cecil;

namespace DotNet.Ildasm
{
    public interface IAssemblyDefinitionResolver
    {
        AssemblyDefinition Resolve(string optionsFilePath);
    }
}