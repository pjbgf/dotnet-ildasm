using DotNet.Ildasm.Configuration;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace DotNet.Ildasm
{
    public class AssemblyDecompiler : IAssemblyDecompiler
    {
        private readonly IOutputWriter _outputWriter;
        private readonly AssemblyReferencesProcessor _assemblyReferencesProcessor;
        private readonly ModuleDirectivesProcessor _moduleDirectivesProcessor;

        public AssemblyDecompiler(string assemblyPath, IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
            _moduleDirectivesProcessor = new ModuleDirectivesProcessor(assemblyPath, outputWriter);
            _assemblyReferencesProcessor = new AssemblyReferencesProcessor(outputWriter);
        }
        
        public void WriteAssemblyExternalReferences(AssemblyDefinition assembly)
        {
            _assemblyReferencesProcessor.WriteAssemblyReferences(assembly.MainModule.AssemblyReferences);
        }

        public void WriteAssemblySection(AssemblyDefinition assembly)
        {
            assembly.WriteIL(_outputWriter);
        }

        public void WriteModuleSection(ModuleDefinition module)
        {
            _moduleDirectivesProcessor.WriteModuleDirective();
            _moduleDirectivesProcessor.WriteModuleVersionId(module.Mvid);
            _moduleDirectivesProcessor.WriteImageBaseDirective();
            _moduleDirectivesProcessor.WriteFileAlignmentDirective();
            _moduleDirectivesProcessor.WriteStackReserveDirective();
            _moduleDirectivesProcessor.WriteSubsystemDirective();
            _moduleDirectivesProcessor.WriteCornFlagsDirective();
        }

        public void WriteModuleTypes(Collection<TypeDefinition> types, ItemFilter itemFilter)
        {
            if (types?.Count > 0)
            {
                var typesProcessor = new TypesProcessor(_outputWriter, itemFilter);
                typesProcessor.Write(types);
            }
        }
    }
}