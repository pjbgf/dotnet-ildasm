using System;
using System.Collections.Generic;
using DotNet.Ildasm.Configuration;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class TypesProcessor
    {
        private readonly IOutputWriter _outputWriter;
        private readonly ItemFilter _itemFilter;

        public TypesProcessor(IOutputWriter outputWriter, ItemFilter itemFilter)
        {
            _outputWriter = outputWriter;
            _itemFilter = itemFilter;
        }

        public void Write(IEnumerable<TypeDefinition> types)
        {
            foreach (var type in types)
            {
                if (string.Compare(type.Name, "<Module>", StringComparison.CurrentCulture) == 0)
                    continue;

                if (string.IsNullOrEmpty(_itemFilter.Class) ||
                    string.Compare(type.Name, _itemFilter.Class, StringComparison.CurrentCulture) == 0)
                    HandleType(type);
            }
        }

        private void HandleType(TypeDefinition type)
        {
            type.WriteILSignature(_outputWriter);
            _outputWriter.WriteLine("{");

            WriteFields(type);
            WriteMethods(type);
            WriteCustomAttributes(type);

            _outputWriter.WriteLine($"}} // End of class {type.FullName}");
        }

        private void WriteCustomAttributes(TypeDefinition type)
        {
            if (type.HasCustomAttributes)
            {
                foreach (var customAttribute in type.CustomAttributes)
                    customAttribute.WriteIL(_outputWriter);
            }
        }

        private void WriteMethods(TypeDefinition type)
        {
            foreach (var method in type.Methods)
            {
                if (string.IsNullOrEmpty(_itemFilter.Method) ||
                    string.Compare(method.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0)
                    HandleMethod(method);
            }
        }

        private void WriteFields(TypeDefinition type)
        {
            if (type.HasFields)
            {
                foreach (var field in type.Fields)
                    field.WriteIL(_outputWriter);
            }
        }

        private void HandleMethod(MethodDefinition method)
        {
            method.WriteILSignature(_outputWriter);
            method.WriteILBody(_outputWriter);
        }
    }
}