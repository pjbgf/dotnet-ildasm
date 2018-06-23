using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public sealed class ConsoleOutputDisassembler : Disassembler
    {
        public ConsoleOutputDisassembler(IAssemblyDecompiler assemblyDataProcessor, IAssemblyDefinitionResolver assemblyResolver) : base(assemblyDataProcessor, assemblyResolver)
        {
        }
        
        public override ExecutionResult Execute(CommandArgument argument, ItemFilter itemFilter)
        {
            var result = base.Execute(argument, itemFilter);
            return result;
        }
    }
}