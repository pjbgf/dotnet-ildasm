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

        public Disassembler Create(CommandArgument argument)
        {
            var outputWriter = GetOutputWriter(argument);
            
            //HACK: Next step for refactoring.
            if (_assemblyDecompiler == null)
                _assemblyDecompiler = new AssemblyDecompiler(argument.Assembly, outputWriter);
            
            if (argument.HasOutputPathSet)
                return new FileOutputDisassembler(_assemblyDecompiler, _assemblyDefinitionResolver);
            
            return new ConsoleOutputDisassembler(_assemblyDecompiler, _assemblyDefinitionResolver);
        }
        
        private IOutputWriter GetOutputWriter(CommandArgument argument)
        {
            if (argument.HasOutputPathSet)
            {
                if (File.Exists(argument.OutputFile))
                {
                    if (!argument.ForceOverwrite)
                        throw new InvalidOperationException($"Error: The file {argument.OutputFile} already exists. Use --force to force it to be overwritten.");
                    
                    File.Delete(argument.OutputFile);
                }

                
                return new AutoIndentOutputWriter(new FileStreamOutputWriter(argument.OutputFile));
            }

            return new AutoIndentOutputWriter(new ConsoleOutputWriter());            
        }
    }
}