using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace DotNet.Ildasm
{
    public class MethodProcessor
    {
        private readonly IOutputWriter _outputWriter;

        public MethodProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteSignature(MethodDefinition method)
        {
            _outputWriter.WriteLine(GetMethodSignature(method));
        }

        public void WriteBody(MethodDefinition method)
        {
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
                    //TODO: External Types should always be preceded by their assembly names #6
                    string output = instruction.ToString();
                    if (instruction.Operand != null)
                    {
                        PropertyInfo fieldType = instruction.Operand.GetType().GetProperty("FieldType");
                        if(fieldType == null)
                        {
                            fieldType = instruction.Operand.GetType().GetProperty("ReturnType");
                        }
                        object value = fieldType.GetValue(instruction.Operand);
                        var fullName = (string)value.GetType().GetProperty("FullName").GetValue(value);
                        var name = (string)value.GetType().GetProperty("Name").GetValue(value);
                        output = output.Replace(fullName, name.ToLowerInvariant());
                    }
                    _outputWriter.WriteLine(output);

                }
            }
            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private string GetMethodSignature(MethodDefinition method)
        {
            var builder = new StringBuilder();
            builder.Append(".method");

            if (method.IsPublic)
                builder.Append(" public");
            else if (method.IsPrivate)
                builder.Append(" private");

            if (method.IsHideBySig)
                builder.Append(" hidebysig");

            if (method.IsNewSlot)
                builder.Append(" newslot");

            if (method.IsAbstract)
                builder.Append(" abstract");

            if (method.IsVirtual)
                builder.Append(" virtual");

            if (method.IsSpecialName)
                builder.Append(" specialname");

            if (method.IsRuntimeSpecialName)
                builder.Append(" rtspecialname");

            if (method.IsFinal)
                builder.Append(" final");

            if (!method.IsStatic)
                builder.Append(" instance");

            builder.Append($" {method.ReturnType.MetadataType.ToString().ToLowerInvariant()}");
            builder.Append($" {method.Name}");

            AppendMethodParameters(method, builder);

            if (method.IsManaged)
                builder.Append(" cil managed");

            return builder.ToString();
        }

        private static void AppendMethodParameters(MethodDefinition method, StringBuilder builder)
        {
            builder.Append("(");

            if (method.HasParameters)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");

                    var parameterDefinition = method.Parameters[i];
                    builder.Append($"{parameterDefinition.ParameterType.MetadataType.ToString().ToLowerInvariant()} ");
                    builder.Append(parameterDefinition.Name);
                }
            }

            builder.Append(")");
        }
    }
}