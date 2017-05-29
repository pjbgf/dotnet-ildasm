using System;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

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
                    _outputWriter.WriteLine(GetInstructionIL(instruction));
                }
            }
            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private string GetInstructionIL(Instruction instruction)
        {
            var instructionWithouOperand = $"IL_{instruction.Offset:x4}: {instruction.OpCode.ToString()}";
            var operand = GetOperandIL(instruction);

            return $"{instructionWithouOperand}{operand}";
        }

        private string GetOperandIL(Instruction instruction)
        {
            if (instruction.Operand == null)
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append(' ');

            if (instruction.OpCode.OperandType == OperandType.InlineString)
            {
                builder.Append('"');
                builder.Append(instruction.Operand);
                builder.Append('"');
            }
            else
            {
                var methodReference = instruction.Operand as MethodReference;
                if (methodReference != null)
                    builder.Append(
                        $"{methodReference.ReturnType.ToILType()} {methodReference.DeclaringType.ToILType()}::{methodReference.Name}{GetMethodCallParameters(methodReference)}");
                else
                    builder.Append(instruction.Operand);
            }

            return builder.ToString();
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
                    builder.Append(parameterDefinition.Name);
                }
            }

            builder.Append(")");
        }

        private static string GetMethodCallParameters(MethodReference method)
        {
            var builder = new StringBuilder();
            builder.Append("(");

            if (method.HasParameters)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");

                    var parameterDefinition = method.Parameters[i];
                    builder.Append($"{parameterDefinition.ParameterType.ToILType()} ");
                    builder.Append(parameterDefinition.Name);
                }
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}