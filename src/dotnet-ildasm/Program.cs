using System;
using System.IO;
using CommandLine;
using DotNet.Ildasm.Adapters;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    class Program
    {
        private static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            result.MapResult(
                PrepareToExecute,
                _ => OnError());
        }

        private static int OnError()
        {
            return -1;
        }

        private static int PrepareToExecute(CommandOptions options)
        {
            var indentationProvider = new IndentationProvider();

            Console.WriteLine();
            if (File.Exists(options.OutputPath) && !options.IsTextOutput)
            {
                if (!options.ForceOutputOverwrite)
                {
                    Console.WriteLine(
                        $"Error: The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                    return -1;
                }

                File.Delete(options.OutputPath);
            }

            var outputWriter = GetOutputWriter(options, indentationProvider);

            return ExecuteDisassembler(options, outputWriter);
        }

        private static int ExecuteDisassembler(CommandOptions options, IOutputWriter outputWriter)
        {
            var assemblyDataProcessor = new AssemblyDataProcessor(options.FilePath, outputWriter);
            var assemblyDefinitionResolver = new AssemblyDefinitionResolver();
            var disassembler = new Disassembler(assemblyDataProcessor, assemblyDefinitionResolver);

            var itemFilter = new ItemFilter(options.ItemFilter);
            var result = disassembler.Execute(options, itemFilter);
            outputWriter.Dispose();

            if (result.Succeeded || !options.IsTextOutput)
            {
                Console.WriteLine(result.Message);
                return 0;
            }

            Console.WriteLine($"Error: {result.Message}");
            return -1;
        }

        private static IOutputWriter GetOutputWriter(CommandOptions options, IndentationProvider indentationProvider)
        {
            IOutputWriter outputWriter = null;

            if (options.IsTextOutput)
            {
                outputWriter = new ConsoleOutputWriter(indentationProvider);
            }
            else
            {
                outputWriter = new FileStreamOutputWriter(indentationProvider, options.OutputPath);
            }
            return outputWriter;
        }
    }
}