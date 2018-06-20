using System.IO;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public sealed class FileOutputDisassembler : Disassembler
    {
        public FileOutputDisassembler(IAssemblyDecompiler assemblyDataProcessor, IAssemblyDefinitionResolver assemblyResolver) : base(assemblyDataProcessor, assemblyResolver)
        {
        }
        
        public override ExecutionResult Execute(CommandOptions options, ItemFilter itemFilter)
        {
            if (!string.IsNullOrEmpty(options.OutputPath) && File.Exists(options.OutputPath))
            {
                if (!options.ForceOutputOverwrite)
                {
                    return new ExecutionResult(false, $"Error: The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                }

                File.Delete(options.OutputPath);
            }
            
            var result = base.Execute(options, itemFilter);
            if (result.Succeeded)
                return new ExecutionResult(true, $"Assembly IL exported to {options.OutputPath}");

            return result;
        }
    }
}