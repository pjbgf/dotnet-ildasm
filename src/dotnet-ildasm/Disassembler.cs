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
        private readonly ModuleDirectivesProcessor _moduleDirectivesProcessor;
        private readonly AssemblySectionProcessor _assemblySectionProcessor;
        private readonly AssemblyReferencesProcessor _assemblyReferencesProcessor;

        public Disassembler(IOutputWriter outputWriter, CommandOptions options, ItemFilter itemFilter)
        {
            _outputWriter = outputWriter;
            _options = options;
            _itemFilter = itemFilter;
            _moduleDirectivesProcessor = new ModuleDirectivesProcessor(_options.FilePath, outputWriter);
            _assemblySectionProcessor = new AssemblySectionProcessor(outputWriter);
            _assemblyReferencesProcessor = new AssemblyReferencesProcessor(outputWriter);
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

                var typesProcessor = new TypesProcessor(_outputWriter, _itemFilter);
                typesProcessor.Write(module.Types);
            }
        }

        private void WriteAssemblySection(AssemblyDefinition assembly)
        {
            _assemblySectionProcessor.WriteAssemblyName(assembly);
            _assemblySectionProcessor.WriteBody(assembly);
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
