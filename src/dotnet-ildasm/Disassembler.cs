using System;
using System.IO;

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

        public ExecutionResult Execute(CommandOptions options, ItemFilter itemFilter)
        {
            var assembly = _assemblyResolver.Resolve(options.FilePath);
            if (assembly == null)
                return new ExecutionResult(false, "Assembly could not be loaded, please check the path and try again.");
            
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
            
            if (!options.IsTextOutput)
                return new ExecutionResult(true, $"Assembly IL exported to {options.OutputPath}");

            return new ExecutionResult(true);
        }
    }
}
