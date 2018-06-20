using System;
using CommandLine;
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
            var result = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            return result.MapResult(
                ExecuteDisassembler,
                _ => OnError());
        }

        private int ExecuteDisassembler(CommandOptions options)
        {
            ExecutionResult executionResult;
            
            try
            {
                var disassembler = factory.Create(options);
                var itemFilter = new ItemFilter(options.ItemFilter);
                
                executionResult = disassembler.Execute(options, itemFilter);
            }
            catch (Exception e)
            {
                executionResult = new ExecutionResult(false, e.Message);
            }
            
            if (executionResult.Message?.Length > 0)
                Console.WriteLine(executionResult.Message);
            
            return executionResult.Succeeded ? 0 : -1;
        }

        private int OnError()
        {
            return -1;
        }
    }
}