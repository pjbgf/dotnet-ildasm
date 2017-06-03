using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class AssemblyDefinitionExtensions
    {
        public static void WriteIL(this AssemblyDefinition assembly, IOutputWriter outputWriter)
        {
            outputWriter.WriteLine($".assembly '{assembly.Name.Name}'");
            outputWriter.WriteLine("{");

            WriteCustomAttributes(assembly, outputWriter);

            outputWriter.WriteLine($".hash algorithm 0x{assembly.Name.HashAlgorithm:X}");
            outputWriter.WriteLine($".ver {assembly.Name.Version.Major}:{assembly.Name.Version.Minor}:{assembly.Name.Version.Revision}:{assembly.Name.Version.Build}");
            outputWriter.WriteLine("}");
        }

        private static void WriteCustomAttributes(AssemblyDefinition assembly, IOutputWriter outputWriter)
        {
            foreach (var customAttribute in assembly.CustomAttributes)
            {
                if (string.Compare(customAttribute.AttributeType.Name, "DebuggableAttribute",
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    continue;

                customAttribute.WriteIL(outputWriter);
            }
        }
    }
}