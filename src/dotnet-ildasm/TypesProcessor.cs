using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Ildasm.Configuration;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    internal sealed class TypesProcessor
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

                if (!IsFilterSet() ||
                    DoesTypeMatchFilter(type) ||
                    DoesTypeContainMethodMatchingFilter(type))
                    HandleType(type);
            }
        }

        private bool DoesTypeMatchFilter(TypeDefinition type)
        {
            return string.Compare(type.Name, _itemFilter.Class, StringComparison.CurrentCulture) == 0;
        }

        private bool DoesTypeContainMethodMatchingFilter(TypeDefinition type)
        {
            return (!string.IsNullOrEmpty(_itemFilter.Method) && type.Methods.Any(x => string.Compare(x.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0));
        }

        private bool IsFilterSet()
        {
            return _itemFilter.HasFilter;
        }

        private void HandleType(TypeDefinition type)
        {
            type.WriteILSignature(_outputWriter);
            _outputWriter.WriteLine("{");

            WriteCustomAttributes(type);
            WriteFields(type);
            WriteMethods(type);
            WriteProperties(type);

            foreach (var nestedType in type.NestedTypes)
                HandleType(nestedType);

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

        private void WriteProperties(TypeDefinition type)
        {
            foreach (var property in type.Properties)
            {
                if (string.IsNullOrEmpty(_itemFilter.Method) ||
                    string.Compare(property.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0)
                    HandleProperty(property);
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

        private void HandleProperty(PropertyDefinition property)
        {
            property.WriteILSignature(_outputWriter);
            property.WriteILBody(_outputWriter);
        }
    }
}