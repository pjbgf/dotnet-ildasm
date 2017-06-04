using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class FieldDefinitionExtensions
    {
        public static void WriteIL(this FieldDefinition field, IOutputWriter writer)
        {
            writer.Write(".field ");
            
            if (field.IsPublic)
                writer.Write("public ");

            if (field.IsPrivate)
                writer.Write("private ");

            if (field.IsStatic)
                writer.Write("static ");

            if (field.IsInitOnly)
                writer.Write("initonly ");

            writer.Write($"{field.FieldType.ToIL()} {EscapeIfNeeded(field.Name)}{Environment.NewLine}");
        }

        private static string EscapeIfNeeded(string fieldName)
        {
            if (fieldName.Contains("<"))
                return $"'{fieldName}'";

            return fieldName;
        }
    }
}