using System;
using System.Linq;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DotNet.Ildasm
{
    public static class PropertyDefinitionExtensions
    {
        public static void WriteILBody(this PropertyDefinition property, IOutputWriter outputWriter)
        {
            outputWriter.WriteLine(String.Empty);
            outputWriter.WriteLine("{");

            property.WriteCustomAttributes(outputWriter);
            property.WriteGetMethod(outputWriter);
            property.WriteSetMethod(outputWriter);

            outputWriter.WriteLine($"}} // End of property {property.FullName}");
        }

        private static void WriteGetMethod(this PropertyDefinition property, IOutputWriter writer)
        {
            var getMethod = property.GetMethod;
            if (getMethod != null)
            {
                var instance = property.HasThis ? "instance " : "";
                writer.WriteLine($".get {instance}default {getMethod.ReturnType.ToIL()} {getMethod.DeclaringType.ToIL()}::{getMethod.Name} ()");
            }
        }

        private static void WriteSetMethod(this PropertyDefinition property, IOutputWriter writer)
        {
            var setMethod = property.SetMethod;
            if (setMethod != null)
            {
                var instance = property.HasThis ? "instance " : "";
                writer.WriteLine($".set {instance}default void {setMethod.DeclaringType.ToIL()}::{setMethod.Name} ({setMethod.Parameters.First().ParameterType.ToIL()} 'value')");
            }
        }

        private static void WriteCustomAttributes(this PropertyDefinition propertyDefinition, IOutputWriter outputWriter)
        {
            foreach (var customAttribute in propertyDefinition.CustomAttributes)
            {
                customAttribute.WriteIL(outputWriter);
            }
        }

        public static void WriteILSignature(this PropertyDefinition property, IOutputWriter writer)
        {
            writer.Write(".property");

            if (property.IsSpecialName)
                writer.Write(" specialname");

            if (property.IsRuntimeSpecialName)
                writer.Write(" rtspecialname");

            if (property.HasThis)
                writer.Write(" instance");

            writer.Write($" {property.PropertyType.ToIL()}");
            writer.Write($" {property.Name} ()");
        }
    }
}