using System;
using System.IO;
using CommandLine;
using DotNet.Ildasm.Adapters;

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
            var outputWriter = GetOutputWriter(options, indentationProvider);

            Console.WriteLine();
            if (File.Exists(options.OutputPath) && !options.ForceOutputOverwrite)
            {
                Console.WriteLine($"Error: The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                return -1;
            }

            DeleteOutputFileWhenForceOutputOverwriteFlagOn(options);

            return ExecuteDisassembler(options, outputWriter);
        }

        private static int ExecuteDisassembler(CommandOptions options, IOutputWriter outputWriter)
        {
            var assemblyDataProcessor = new AssemblyDataProcessor(options.FilePath, outputWriter);
            var assemblyDefinitionResolver = new AssemblyDefinitionResolver();
            var disassembler = new Disassembler(assemblyDataProcessor, assemblyDefinitionResolver);

            var itemFilter = new ItemFilter(options.ItemFilter);
            var result = disassembler.Execute(options, itemFilter);

            if (result.Succeeded || !options.IsTextOutput)
            {
                Console.WriteLine(result.Message);
                return 0;
            }

            Console.WriteLine($"Error: {result.Message}");
            return -1;
        }

        private static void DeleteOutputFileWhenForceOutputOverwriteFlagOn(CommandOptions options)
        {
            if (!options.IsTextOutput && options.ForceOutputOverwrite)
            {
                File.Delete(options.OutputPath);
            }
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
                outputWriter = new FileOutputWriter(indentationProvider, options.OutputPath);
            }
            return outputWriter;
        }
    }
}