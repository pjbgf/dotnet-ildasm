using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    sealed class Disassembler
    {
        private readonly IOutputWriter _outputWriter;

        internal Disassembler(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        internal void Execute(CommandOptions options)
        {
            var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(options.FilePath);

            WriteExterns(assembly);
            WriteAssemblyData(assembly);
        }

        private void WriteExterns(AssemblyDefinition assembly)
        {
            foreach (var reference in assembly.MainModule.AssemblyReferences)
            {
                _outputWriter.WriteLine();
                _outputWriter.WriteLine($".assembly extern {reference.Name}");
                _outputWriter.WriteLine("{");
                //TODO: Show publickeytoken in HEX
                _outputWriter.WriteLine($"// .publickeytoken {ToBinary(reference.PublicKeyToken)} // Needs proper formatting");
                _outputWriter.WriteLine($".ver {reference.Version.Major}:{reference.Version.Minor}:{reference.Version.Revision}:{reference.Version.Build}");
                _outputWriter.WriteLine("}");
            }
        }

        private String ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => string.Format("{0:X}", Convert.ToString(byt, 2))));
        }

        private void WriteAssemblyData(AssemblyDefinition assembly)
        {
            _outputWriter.WriteLine();
            _outputWriter.WriteLine($".assembly '{ assembly.Name.Name }'");
            _outputWriter.WriteLine("{");

            foreach (var customAttribute in assembly.CustomAttributes)
            {
                if (String.Compare(customAttribute.AttributeType.Name, "DebuggableAttribute",
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    _outputWriter.WriteLine("// The following custom attribute is added automatically for debugging purposes, do not uncomment. ");
                    _outputWriter.Write("//");
                }

                //TODO: Check whether custom attributes can NOT be instance 
                //TODO: Convert .Net types onto IL types
                //TODO: Add custom attributes parameter values
                _outputWriter.WriteLine($".custom instance {customAttribute.Constructor.ToString()}");
            }

            _outputWriter.WriteLine($".hash algorithm 0x{assembly.Name.HashAlgorithm.ToString("X")}");
            _outputWriter.WriteLine($".ver {assembly.Name.Version.Major}:{assembly.Name.Version.Minor}:{assembly.Name.Version.Revision}:{assembly.Name.Version.Build}");
            _outputWriter.WriteLine("}");

            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.Types)
                {
                    if (string.Compare(type.Name, "<Module>") == 0)
                        HandleModule(type);
                    else
                        HandleType(type);
                }
            }
        }

        private void HandleType(TypeDefinition type)
        {
            _outputWriter.WriteLine();
            WriteTypeSignature(type);
            _outputWriter.WriteLine("{");

            foreach (var method in type.Methods)
                HandleMethod(method);

            _outputWriter.WriteLine();
            _outputWriter.WriteLine($"}} // End of class {type.FullName}");
        }

        private void HandleMethod(MethodDefinition method)
        {
            var ilProcessor = method.Body.GetILProcessor();

            _outputWriter.WriteLine();
            WriteMethodSignature(method);
            _outputWriter.WriteLine("{");

            if (method.DeclaringType.Module.EntryPoint == method)
                _outputWriter.WriteLine(".entrypoint");

            _outputWriter.WriteLine($"// Code size {method.Body.CodeSize}");
            _outputWriter.WriteLine($".maxstack {method.Body.MaxStackSize}");
            foreach (var instruction in ilProcessor.Body.Instructions)
            {
                _outputWriter.WriteLine(instruction.ToString());
            }

            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private void WriteTypeSignature(TypeDefinition type)
        {
            _outputWriter.Write(".class");

            if (type.IsPublic)
                _outputWriter.Write(" public");
            else if (type.IsNotPublic)
                _outputWriter.Write(" private");

            if (type.IsAutoLayout)
                _outputWriter.Write(" auto");

            if (type.IsAnsiClass)
                _outputWriter.Write(" ansi");

            if (type.IsBeforeFieldInit)
                _outputWriter.Write(" beforefieldinit");

            //TODO: Signature to be IL compatible
            _outputWriter.Write($" {type.FullName}");
            //TODO: Add assembly that implements the type
            _outputWriter.Write($" extends {type.BaseType.FullName}");
        }

        private void WriteMethodSignature(MethodDefinition method)
        {
            _outputWriter.Write(".method");

            if (method.IsPublic)
                _outputWriter.Write(" public");
            else if (method.IsPrivate)
                _outputWriter.Write(" private");

            if (method.IsHideBySig)
                _outputWriter.Write(" hidebysig");

            if (method.IsSpecialName)
                _outputWriter.Write(" specialname");

            if (method.IsRuntimeSpecialName)
                _outputWriter.Write(" rtspecialname");

            if (!method.IsStatic)
                _outputWriter.Write(" instance");

            //TODO: Signature to be IL compatible
            _outputWriter.Write($" {method.FullName}");

            if (method.IsManaged)
                _outputWriter.Write(" cil managed");
        }

        private void HandleModule(TypeDefinition type)
        {
            _outputWriter.WriteLine("");
            _outputWriter.WriteLine($".module '{ type.Module.Assembly.Name.Name }'");
            _outputWriter.WriteLine($"// MVID: {{{type.Module.Mvid}}}");

            //TODO: Load module information
            _outputWriter.WriteLine($"// .imagebase 0x000000 (Currently not supported)");
            _outputWriter.WriteLine($"// .file alignment 0x000000 (Currently not supported)");
            _outputWriter.WriteLine($"// .stackreserve 0x000000 (Currently not supported)");

            //TODO: Load subsystem from actual memory instead of assume it
            if (type.Module.Kind == ModuleKind.Console || type.Module.Kind == ModuleKind.Windows)
                _outputWriter.WriteLine($"//.subsystem 0x{ GetSubsystem(type.Module.Kind).ToString("x3") }");

            _outputWriter.WriteLine($"// .cornflags 0x000000 (Currently not supported)");
            _outputWriter.WriteLine($"// image base:  ");
        }

        private uint GetSubsystem(ModuleKind moduleKind)
        {
            if (moduleKind == ModuleKind.Console)
                return 0x003;

            if (moduleKind == ModuleKind.Windows)
                return 0x002;

            return 0;
        }
    }
}
