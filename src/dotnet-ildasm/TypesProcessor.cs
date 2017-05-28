using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class TypesProcessor
    {
        private static MethodProcessor _methodProcessor;
        private readonly IOutputWriter _outputWriter;
        private readonly ItemFilter _itemFilter;
        private readonly TypeProcessor _typeProcessor;

        public TypesProcessor(IOutputWriter outputWriter, ItemFilter itemFilter)
        {
            _methodProcessor = new MethodProcessor(outputWriter);
            _outputWriter = outputWriter;
            _itemFilter = itemFilter;
            _typeProcessor = new TypeProcessor();
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
            _outputWriter.WriteLine(_typeProcessor.GetTypeSignature(type));
            _outputWriter.WriteLine("{");

            foreach (var method in type.Methods)
            {
                if (string.IsNullOrEmpty(_itemFilter.Method) ||
                    string.Compare(method.Name, _itemFilter.Method, StringComparison.CurrentCulture) == 0)
                    HandleMethod(method);
            }

            _outputWriter.WriteLine($"}} // End of class {type.FullName}");
        }

        private void HandleMethod(MethodDefinition method)
        {
            _methodProcessor.WriteSignature(method);
            _methodProcessor.WriteBody(method);
        }
    }
}