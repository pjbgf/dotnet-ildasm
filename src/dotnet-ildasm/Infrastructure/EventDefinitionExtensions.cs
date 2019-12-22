using System;
using System.Linq;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public static class EventDefinitionExtensions
    {
        public static void WriteILBody(this EventDefinition eventDefinition, IOutputWriter outputWriter)
        {
            outputWriter.WriteLine(String.Empty);
            outputWriter.WriteLine("{");

            eventDefinition.WriteCustomAttributes(outputWriter);
            eventDefinition.WriteAddMethod(outputWriter);
            eventDefinition.WriteRemoveMethod(outputWriter);

            outputWriter.WriteLine($"}} // End of property {eventDefinition.FullName}");
        }

        private static void WriteRemoveMethod(this EventDefinition eventDefinition, IOutputWriter writer)
        {
            var removeMethod = eventDefinition.RemoveMethod;
            if (removeMethod != null)
            {
                var instance = removeMethod.HasThis ? "instance " : "";
                writer.WriteLine($".removeon {instance}default void {removeMethod.DeclaringType.ToIL()}::{removeMethod.Name} (class {removeMethod.Parameters.First().ParameterType.ToIL()} 'value')");
            }
        }

        private static void WriteAddMethod(this EventDefinition eventDefinition, IOutputWriter writer)
        {
            var addMethod = eventDefinition.AddMethod;
            if (addMethod != null)
            {
                var instance = addMethod.HasThis ? "instance " : "";
                writer.WriteLine($".addon {instance}default void {addMethod.DeclaringType.ToIL()}::{addMethod.Name} (class {addMethod.Parameters.First().ParameterType.ToIL()} 'value')");
            }
        }

        private static void WriteCustomAttributes(this EventDefinition eventDefinition, IOutputWriter outputWriter)
        {
            foreach (var customAttribute in eventDefinition.CustomAttributes)
            {
                customAttribute.WriteIL(outputWriter);
            }
        }

        public static void WriteILSignature(this EventDefinition eventDefinition, IOutputWriter writer)
        {
            writer.Write(".event class");

            if (eventDefinition.IsSpecialName)
                writer.Write(" specialname");

            if (eventDefinition.IsRuntimeSpecialName)
                writer.Write(" rtspecialname");

            writer.Write($" {eventDefinition.EventType.ToIL()}");
            writer.Write($" {eventDefinition.Name}");
        }
    }
}