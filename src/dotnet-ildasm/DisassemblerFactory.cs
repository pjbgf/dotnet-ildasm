using System;
using System.IO;
using DotNet.Ildasm.Adapters;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public class DisassemblerFactory : IDisassemblerFactory
    {
        private readonly IAssemblyDefinitionResolver _assemblyDefinitionResolver;
        private IAssemblyDecompiler _assemblyDecompiler;

        public DisassemblerFactory(IAssemblyDefinitionResolver assemblyDefinitionResolver)
        {
            _assemblyDefinitionResolver = assemblyDefinitionResolver;
        }

        public DisassemblerFactory(IAssemblyDefinitionResolver assemblyDefinitionResolver, IAssemblyDecompiler assemblyDecompiler)
        {
            _assemblyDefinitionResolver = assemblyDefinitionResolver;
            _assemblyDecompiler = assemblyDecompiler;
        }

        public Disassembler Create(CommandOptions options)
        {
            var outputWriter = GetOutputWriter(options);
            
            //HACK: Next step for refactoring.
            if (_assemblyDecompiler == null)
                _assemblyDecompiler = new AssemblyDecompiler(options.FilePath, outputWriter);
            
            if (options.HasOutputPathSet)
                return new FileOutputDisassembler(_assemblyDecompiler, _assemblyDefinitionResolver);
            
            return new ConsoleOutputDisassembler(_assemblyDecompiler, _assemblyDefinitionResolver);
        }
        
        private IOutputWriter GetOutputWriter(CommandOptions options)
        {
            if (options.HasOutputPathSet)
            {
                if (File.Exists(options.OutputPath))
                {
                    if (!options.ForceOutputOverwrite)
                        throw new InvalidOperationException($"Error: The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                    
                    File.Delete(options.OutputPath);
                }

                
                return new AutoIndentOutputWriter(new FileStreamOutputWriter(options.OutputPath));
            }

            return new AutoIndentOutputWriter(new ConsoleOutputWriter());            
        }
    }
}