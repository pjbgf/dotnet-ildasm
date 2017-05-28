using Mono.Cecil;
using Mono.Collections.Generic;

namespace DotNet.Ildasm
{
    public class AssemblyReferencesProcessor
    {
        private readonly IOutputWriter _outputWriter;

        public AssemblyReferencesProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteAssemblyReferences(Collection<AssemblyNameReference> assemblyReferences)
        {
            foreach (var reference in assemblyReferences)
            {
                _outputWriter.WriteLine($".assembly extern {reference.Name}");
                _outputWriter.WriteLine("{");
                _outputWriter.WriteLine($".publickeytoken ( {reference.PublicKeyToken.ToHexadecimal()} )");
                _outputWriter.WriteLine(
                    $".ver {reference.Version.Major}:{reference.Version.Minor}:{reference.Version.Revision}:{reference.Version.Build}");
                _outputWriter.WriteLine("}");
            }
        }
    }
}