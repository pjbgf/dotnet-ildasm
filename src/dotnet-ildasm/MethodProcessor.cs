using System.Linq;
using System.Reflection;
using System.Text;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class MethodProcessor
    {
        private readonly IOutputWriter _outputWriter;
        private readonly InstructionProcessor _instructionProcessor;

        public MethodProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
            _instructionProcessor = new InstructionProcessor(_outputWriter);
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

                WriteLocalVariablesIfNeeded(method);

                var ilProcessor = method.Body.GetILProcessor();
                foreach (var instruction in ilProcessor.Body.Instructions)
                {
                    _instructionProcessor.WriteInstruction(instruction);
                }
            }

            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private void WriteLocalVariablesIfNeeded(MethodDefinition method)
        {
            if (method.Body.InitLocals)
            {
                if (method.Body.Variables.Count == 1)
                    _outputWriter.WriteLine($".locals init(class {method.Body.Variables.First().VariableType.ToILType()} V_0)");
                else if(method.Body.Variables.Count > 1)
                {
                    int parameterIndex = 0;
                    _outputWriter.WriteLine(
                        $".locals init(class {(string.Join($"V_{parameterIndex}, ", method.Body.Variables.Select(x => x.VariableType.ToILType())))})");
                }
            }
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
            else
                builder.Append(" static");

            builder.Append($" {method.ReturnType.ToILType()}");
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
                    builder.Append($"{parameterDefinition.ParameterType.ToILType()} ");

                    if (parameterDefinition.Name == "value")
                        builder.Append($"'{parameterDefinition.Name}'");
                    else 
                        builder.Append(parameterDefinition.Name);
                }
            }

            builder.Append(")");
        }
    }
}