using System;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    internal class Program
    {
        IDisassemblerFactory factory = new DisassemblerFactory(new AssemblyDefinitionResolver());
        
        static int Main(string[] args)
        {
            return new Program().Execute(args);
        }
        
        internal int Execute(string[] args)
        {
            var handler = new CommandHandler(ExecuteDisassembler);
            
            return handler.Handle(args);
        }

        private int ExecuteDisassembler(CommandArgument argument)
        {
            ExecutionResult executionResult;
            
            try
            {
                using (var disassembler = factory.Create(argument))
                {
                    var itemFilter = new ItemFilter(argument.Item);

                    executionResult = disassembler.Execute(argument, itemFilter);
                }
            }
            catch (Exception e)
            {
                executionResult = new ExecutionResult(false, e.Message);
            }
            
            if (executionResult.Message?.Length > 0)
                Console.WriteLine(executionResult.Message);
            
            return executionResult.Succeeded ? 0 : -1;
        }
    }
}