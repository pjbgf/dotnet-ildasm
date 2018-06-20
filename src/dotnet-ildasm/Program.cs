using System;
using System.IO;
using CommandLine;
using DotNet.Ildasm.Adapters;
using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    internal class Program
    {
        static int Main(string[] args)
        {
            return new Program().Execute(args);
        }
        
        internal int Execute(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            return result.MapResult(
                PrepareToExecute,
                _ => OnError());
        }

        private int OnError()
        {
            return -1;
        }

        private int PrepareToExecute(CommandOptions options)
        {
            Console.WriteLine();
            if (!string.IsNullOrEmpty(options.OutputPath) && File.Exists(options.OutputPath))
            {
                if (!options.ForceOutputOverwrite)
                {
                    Console.WriteLine(
                        $"Error: The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                    return -1;
                }

                File.Delete(options.OutputPath);
            }

            var outputWriter = GetOutputWriter(options);

            return ExecuteDisassembler(options, outputWriter);
        }

        private int ExecuteDisassembler(CommandOptions options, IOutputWriter outputWriter)
        {
            var assemblyDataProcessor = new AssemblyDecompiler(options.FilePath, outputWriter);
            var assemblyDefinitionResolver = new AssemblyDefinitionResolver();
            var disassembler = new Disassembler(assemblyDataProcessor, assemblyDefinitionResolver);

            var itemFilter = new ItemFilter(options.ItemFilter);
            var result = disassembler.Execute(options, itemFilter);
            outputWriter.Dispose();

            if (result.Succeeded || string.IsNullOrEmpty(options.OutputPath))
            {
                Console.WriteLine(result.Message);
                return 0;
            }

            Console.WriteLine($"Error: {result.Message}");
            return -1;
        }

        private IOutputWriter GetOutputWriter(CommandOptions options)
        {
            IOutputWriter outputWriter = null;

            if (string.IsNullOrEmpty(options.OutputPath))
            {
                outputWriter = new ConsoleOutputWriter();
            }
            else
            {
                outputWriter = new FileStreamOutputWriter(options.OutputPath);
            }

            return new AutoIndentOutputWriter(outputWriter);
        }
    }
}