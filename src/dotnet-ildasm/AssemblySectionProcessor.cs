using System;
using System.Linq;
using System.Text;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class AssemblySectionProcessor
    {
        private readonly IOutputWriter _outputWriter;

        public AssemblySectionProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteAssemblyName(AssemblyDefinition assembly)
        {
            _outputWriter.WriteLine($".assembly '{assembly.Name.Name}'");
        }

        public void WriteBody(AssemblyDefinition assembly)
        {
            _outputWriter.WriteLine("{");

            foreach (var customAttribute in assembly.CustomAttributes)
            {
                if (String.Compare(customAttribute.AttributeType.Name, "DebuggableAttribute",
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    continue;

                _outputWriter.Write(customAttribute.ToIL());
            }

            _outputWriter.WriteLine($".hash algorithm 0x{assembly.Name.HashAlgorithm.ToString("X")}");
            _outputWriter.WriteLine(
                $".ver {assembly.Name.Version.Major}:{assembly.Name.Version.Minor}:{assembly.Name.Version.Revision}:{assembly.Name.Version.Build}");
            _outputWriter.WriteLine("}");
        }
    }
}