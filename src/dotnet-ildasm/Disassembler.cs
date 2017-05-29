using System;

namespace DotNet.Ildasm
{
    public sealed class Disassembler
    {
        private readonly IAssemblyDataProcessor _assemblyDataProcessor;
        private readonly IAssemblyDefinitionResolver _assemblyResolver;

        public Disassembler(IAssemblyDataProcessor assemblyDataProcessor, IAssemblyDefinitionResolver assemblyResolver)
        {
            _assemblyDataProcessor = assemblyDataProcessor;
            _assemblyResolver = assemblyResolver;
        }

        public void Execute(CommandOptions options, ItemFilter itemFilter)
        {
            var assembly = _assemblyResolver.Resolve(options.FilePath);

            if (!itemFilter.HasFilter)
            {
                _assemblyDataProcessor.WriteAssemblyExternalReferences(assembly);
                _assemblyDataProcessor.WriteAssemblySection(assembly);
            }
            
            foreach (var module in assembly.Modules)
            {
                if (!itemFilter.HasFilter)
                    _assemblyDataProcessor.WriteModuleSection(module);
                
                _assemblyDataProcessor.WriteModuleTypes(module.Types, itemFilter);
            }
        }
    }
}
