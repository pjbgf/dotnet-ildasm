using System;
using System.IO;
using CommandLine;
using DotNet.Ildasm.Adapters;

namespace DotNet.Ildasm
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            result.MapResult(
                RunIldasm,
                _ => ShowInfo());
        }

        private static int ShowInfo()
        {
            return -1;
        }

        private static int RunIldasm(CommandOptions options)
        {
            var indentationProvider = new IndentationProvider();
            IOutputWriter outputWriter = null;

            if (options.IsTextOutput)
            {
                outputWriter = new ConsoleOutputWriter(indentationProvider);
            }
            else
            {
                if (string.IsNullOrEmpty(options.OutputPath))
                    options.OutputPath = Path.GetFileNameWithoutExtension(options.FilePath) + ".il";

                if (File.Exists(options.OutputPath) && !options.ForceOutputOverwrite)
                {
                    Console.WriteLine($"The file {options.OutputPath} already exists. Use --force to force it to be overwritten.");
                }
                else
                {
                    File.Delete(options.OutputPath);
                }

                outputWriter = new FileOutputWriter(indentationProvider, options.OutputPath);
            }

            var itemFilter = new ItemFilter(options.ItemFilter);
            var assemblyDataProcessor = new AssemblyDataProcessor(options.FilePath, outputWriter);
            var assemblyDefinitionResolver = new AssemblyDefinitionResolver();
            
            new Disassembler(assemblyDataProcessor, assemblyDefinitionResolver).Execute(options, itemFilter);

            return 0;
        }
    }
}