using System;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class TypeDefinitionExtensions
    {
        public static void WriteILSignature(this TypeDefinition typeDefinition, IOutputWriter outputWriter)
        {
            outputWriter.Write(".class");

            if (typeDefinition.IsNested)
            {
                if (typeDefinition.IsNestedPublic)
                    outputWriter.Write(" nested public");
                if (typeDefinition.IsNestedPrivate)
                    outputWriter.Write(" nested private");
            } else {
                if (typeDefinition.IsPublic)
                    outputWriter.Write(" public");
                else
                    outputWriter.Write(" private");
            }

            if (typeDefinition.IsSequentialLayout)
                outputWriter.Write(" sequential");

            if (typeDefinition.IsInterface)
                outputWriter.Write(" interface");

            if (typeDefinition.IsAbstract)
                outputWriter.Write(" abstract");

            if (typeDefinition.IsAutoLayout)
                outputWriter.Write(" auto");

            if (typeDefinition.IsAnsiClass)
                outputWriter.Write(" ansi");

            if (typeDefinition.IsSealed)
                outputWriter.Write(" sealed");

            if (typeDefinition.IsBeforeFieldInit)
                outputWriter.Write(" beforefieldinit");

            if (typeDefinition.IsNested)
                outputWriter.Write($" {typeDefinition.Name}");
            else
                outputWriter.Write($" {typeDefinition.FullName}");

            if (typeDefinition.BaseType != null)
                outputWriter.Write(
                    $" extends {typeDefinition.BaseType.ToIL()}");

            if (typeDefinition.HasInterfaces)
                outputWriter.Write(
                    $" implements {string.Join(", ", typeDefinition.Interfaces.Select(x => x.InterfaceType.ToIL()))}");

            outputWriter.Write(Environment.NewLine);
        }
    }
}