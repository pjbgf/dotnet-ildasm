using System;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public abstract class Disassembler : IDisposable
    {
        private readonly IAssemblyDecompiler _assemblyDecompiler;
        private readonly IAssemblyDefinitionResolver _assemblyResolver;

        protected Disassembler(IAssemblyDecompiler assemblyDataProcessor, IAssemblyDefinitionResolver assemblyResolver)
        {
            _assemblyDecompiler = assemblyDataProcessor;
            _assemblyResolver = assemblyResolver;
        }

        public virtual ExecutionResult Execute(CommandArgument argument, ItemFilter itemFilter)
        {
            var assembly = _assemblyResolver.Resolve(argument.Assembly);
            if (assembly == null)
                return new ExecutionResult(false, "Error: Assembly could not be loaded, please check the path and try again.");
            
            if (!itemFilter.HasFilter)
            {
                _assemblyDecompiler.WriteAssemblyExternalReferences(assembly);
                _assemblyDecompiler.WriteAssemblySection(assembly);
            }
            
            foreach (var module in assembly.Modules)
            {
                if (!itemFilter.HasFilter)
                    _assemblyDecompiler.WriteModuleSection(module);
                
                _assemblyDecompiler.WriteModuleTypes(module.Types, itemFilter);
            }

            return new ExecutionResult(true);
        }

        private void ReleaseUnmanagedResources()
        {
            _assemblyDecompiler?.Dispose();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Disassembler()
        {
            ReleaseUnmanagedResources();
        }
    }
}
