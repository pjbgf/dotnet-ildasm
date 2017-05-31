using System;
using System.Linq;
using System.Text;
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

                WriteCustomAttribute(customAttribute);
            }

            _outputWriter.WriteLine($".hash algorithm 0x{assembly.Name.HashAlgorithm.ToString("X")}");
            _outputWriter.WriteLine(
                $".ver {assembly.Name.Version.Major}:{assembly.Name.Version.Minor}:{assembly.Name.Version.Revision}:{assembly.Name.Version.Build}");
            _outputWriter.WriteLine("}");
        }

        public void WriteCustomAttribute(CustomAttribute customAttribute)
        {
            _outputWriter.Write(
                $".custom instance void {GetFullTypeName(customAttribute.AttributeType)}::{customAttribute.Constructor.Name}");
            WriteConstructorArguments(customAttribute);
        }

        private void WriteConstructorArguments(CustomAttribute customAttribute)
        {
            var argument = customAttribute.ConstructorArguments.FirstOrDefault();

            if (!customAttribute.HasConstructorArguments)
                _outputWriter.Write("()");
            else
                _outputWriter.Write($"({argument.Type.MetadataType.ToString().ToLowerInvariant()})");

            _outputWriter.WriteLine($"= ( {BitConverter.ToString(customAttribute.GetBlob()).Replace("-", " ")} )");
        }

        public string GetFullTypeName(TypeReference typeReference)
        {
            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }
    }
}