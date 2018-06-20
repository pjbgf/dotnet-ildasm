using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public sealed class ConsoleOutputDisassembler : Disassembler
    {
        public ConsoleOutputDisassembler(IAssemblyDecompiler assemblyDataProcessor, IAssemblyDefinitionResolver assemblyResolver) : base(assemblyDataProcessor, assemblyResolver)
        {
        }
        
        public override ExecutionResult Execute(CommandOptions options, ItemFilter itemFilter)
        {
            var result = base.Execute(options, itemFilter);
            return result;
        }
    }
}