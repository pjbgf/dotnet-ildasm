using System;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public sealed partial class Disassembler
    {
        private readonly IOutputWriter _outputWriter;
        private readonly CommandOptions _options;
        private readonly ItemFilter _itemFilter;
        private readonly CilHelper _cilHelper;
        private readonly ModuleDirectivesProcessor _moduleDirectivesProcessor;
        private readonly MethodProcessor _methodProcessor;
        private readonly AssemblySectionProcessor _assemblySectionProcessor;
        private readonly AssemblyReferencesProcessor _assemblyReferencesProcessor;

        public Disassembler(IOutputWriter outputWriter, CommandOptions options, ItemFilter itemFilter, CilHelper cilHelper)
        {
            _outputWriter = outputWriter;
            _options = options;
            _itemFilter = itemFilter;
            _cilHelper = cilHelper;
            _moduleDirectivesProcessor = new ModuleDirectivesProcessor(_options.FilePath, _outputWriter);
            _methodProcessor = new MethodProcessor(_outputWriter);
            _assemblySectionProcessor = new AssemblySectionProcessor(_outputWriter);
            _assemblyReferencesProcessor = new AssemblyReferencesProcessor(_outputWriter);
        }

        public void Execute()
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(_options.FilePath);

            WriteAssemblyData(assembly);
        }

        private void WriteAssemblyExternalReferences(AssemblyDefinition assembly)
        {
            _assemblyReferencesProcessor.Write(assembly.MainModule.AssemblyReferences);
        }

        private void WriteAssemblyData(AssemblyDefinition assembly)
        {
            if (!_itemFilter.HasFilter)
            {
                WriteAssemblyExternalReferences(assembly);
                WriteAssemblySection(assembly);
            }

            foreach (var module in assembly.Modules)
            {
                if (!_itemFilter.HasFilter)
                    WriteModuleDirectives(module.Mvid);

                foreach (var type in module.Types)
                {
                    if (string.Compare(type.Name, "<Module>") == 0)
                        continue;

                    if (string.IsNullOrEmpty(_itemFilter.Class) || string.Compare(type.Name, _itemFilter.Class, StringComparison.CurrentCulture) == 0)
                        HandleType(type);
                }
            }
        }

        private void HandleType(TypeDefinition type)
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteLine(_cilHelper.GetTypeSignature(type));
            _outputWriter.WriteLine("{");

            foreach (var method in type.Methods)
            {
                if (string.IsNullOrEmpty(_itemFilter.Method) || string.Compare(method.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0)
                    HandleMethod(method);
            }

            _outputWriter.WriteLine();
            _outputWriter.WriteLine($"}} // End of class {type.FullName}");
        }

        private void WriteAssemblySection(AssemblyDefinition assembly)
        {
            _assemblySectionProcessor.WriteAssemblyName(assembly);
            _assemblySectionProcessor.WriteBody(assembly);
        }

        private void HandleMethod(MethodDefinition method)
        {
            _methodProcessor.WriteSignature(method);
            _methodProcessor.WriteBody(method);
        }

        private void WriteModuleDirectives(Guid moduleVersionId)
        {
            _moduleDirectivesProcessor.WriteModuleDirective();
            _moduleDirectivesProcessor.WriteModuleVersionId(moduleVersionId);
            _moduleDirectivesProcessor.WriteImageBaseDirective();
            _moduleDirectivesProcessor.WriteFileAlignmentDirective();
            _moduleDirectivesProcessor.WriteStackReserveDirective();
            _moduleDirectivesProcessor.WriteSubsystemDirective();
            _moduleDirectivesProcessor.WriteCornFlagsDirective();
        }
    }
}
