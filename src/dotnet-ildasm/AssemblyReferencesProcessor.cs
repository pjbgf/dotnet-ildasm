using Mono.Cecil;
using Mono.Collections.Generic;
using DotNet.Ildasm.Infrastructure;

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
                reference.WriteIL(_outputWriter);
            }
        }
    }
}