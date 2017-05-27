﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public sealed class Disassembler
    {
        private readonly IOutputWriter _outputWriter;
        private readonly CommandOptions _options;
        private readonly ItemFilter _itemFilter;
        private readonly CilHelper _cilHelper;

        public Disassembler(IOutputWriter outputWriter, CommandOptions options, ItemFilter itemFilter, CilHelper cilHelper)
        {
            _outputWriter = outputWriter;
            _options = options;
            _itemFilter = itemFilter;
            _cilHelper = cilHelper;
        }

        public void Execute()
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(_options.FilePath);

            WriteAssemblyData(assembly);
        }

        private void WriteExterns(AssemblyDefinition assembly)
        {
            foreach (var reference in assembly.MainModule.AssemblyReferences)
            {
                _outputWriter.WriteLine();
                _outputWriter.WriteLine($".assembly extern { reference.Name }");
                _outputWriter.WriteLine("{");
                _outputWriter.WriteLine($".publickeytoken ( { ExtractValueInHex(reference.PublicKeyToken) } )");
                _outputWriter.WriteLine($".ver { reference.Version.Major }:{ reference.Version.Minor }:{ reference.Version.Revision }:{ reference.Version.Build }");
                _outputWriter.WriteLine("}");
            }
        }

        private string ExtractValueInHex(Byte[] data)
        {
            return BitConverter.ToString(data);
        }

        private void WriteAssemblyData(AssemblyDefinition assembly)
        {
            if (!_itemFilter.HasFilter)
            {
                WriteExterns(assembly);
                WriteAssemblySection(assembly);
            }

            foreach (var module in assembly.Modules)
            {
                if (!_itemFilter.HasFilter)
                    HandleModule(module);

                foreach (var type in module.Types)
                {
                    if (string.Compare(type.Name, "<Module>") == 0)
                        continue;

                    if (string.IsNullOrEmpty(_itemFilter.Class) || string.Compare(type.Name, _itemFilter.Class, StringComparison.CurrentCulture) == 0)
                        HandleType(type);
                }
            }
        }

        private void WriteAssemblySection(AssemblyDefinition assembly)
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteLine($".assembly '{assembly.Name.Name}'");
            _outputWriter.WriteLine("{");

            foreach (var customAttribute in assembly.CustomAttributes)
            {
                if (String.Compare(customAttribute.AttributeType.Name, "DebuggableAttribute",
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                    continue;

                _outputWriter.WriteLine(_cilHelper.GetCustomAttribute(customAttribute));
            }

            _outputWriter.WriteLine($".hash algorithm 0x{assembly.Name.HashAlgorithm.ToString("X")}");
            _outputWriter.WriteLine(
                $".ver {assembly.Name.Version.Major}:{assembly.Name.Version.Minor}:{assembly.Name.Version.Revision}:{assembly.Name.Version.Build}");
            _outputWriter.WriteLine("}");
        }

        private void HandleType(TypeDefinition type)
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteLine(_cilHelper.GetTypeSignature(type));
            _outputWriter.WriteLine("{");

            foreach (var method in type.Methods)
            {
                if (string.IsNullOrEmpty(_itemFilter.Method) || string.Compare(method.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0)
                    HandleMethod(method);
            }

            _outputWriter.WriteLine();
            _outputWriter.WriteLine($"}} // End of class {type.FullName}");
        }

        private void HandleMethod(MethodDefinition method)
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteLine(_cilHelper.GetMethodSignature(method));
            _outputWriter.WriteLine("{");

            if (method.DeclaringType.Module.EntryPoint == method)
                _outputWriter.WriteLine(".entrypoint");

            if (method.HasBody)
            {
                _outputWriter.WriteLine($"// Code size {method.Body.CodeSize}");
                _outputWriter.WriteLine($".maxstack {method.Body.MaxStackSize}");

                var ilProcessor = method.Body.GetILProcessor();
                foreach (var instruction in ilProcessor.Body.Instructions)
                {
                    //TODO: Use IL types instead of .Net types #2
                    //TODO: External Types should always be preceded by their assembly names #6
                    _outputWriter.WriteLine(instruction.ToString());
                }
            }
            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private void HandleModule(ModuleDefinition module)
        {
            _outputWriter.WriteLine("");
            _outputWriter.WriteLine($".module '{ Path.GetFileName(_options.FilePath) }'");
            _outputWriter.WriteLine($"// MVID: {{{module.Mvid}}}");

            var peHeader = PeHeaderHelper.GetPeHeaders(_options.FilePath);
            _outputWriter.WriteLine(PeHeaderHelper.GetImageBaseDirective(peHeader.PEHeader));
            _outputWriter.WriteLine(PeHeaderHelper.GetFileAlignmentDirective(peHeader.PEHeader));
            _outputWriter.WriteLine(PeHeaderHelper.GetStackReserveDirective(peHeader.PEHeader));
            _outputWriter.WriteLine(PeHeaderHelper.GetSubsystemDirective(peHeader.PEHeader));
            _outputWriter.WriteLine(PeHeaderHelper.GetCornFlagsDirective(peHeader));
        }
    }
}
