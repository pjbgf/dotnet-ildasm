using System;
using System.Text;
using DotNet.Ildasm;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DotNet.Ildasm
{
    public class InstructionProcessor
    {
        private readonly IOutputWriter _outputWriter;

        public InstructionProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteInstruction(Instruction instruction)
        {
            var exportedIL = GetInstructionIL(instruction);
            _outputWriter.WriteLine(exportedIL);
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
                return String.Empty;

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
                switch (instruction.Operand)
                {
                    case Instruction subInstruction:
                        builder.Append($"IL_{subInstruction.Offset:x4}");
                        break;
                    case MethodReference methodReference:
                        var instanceString = methodReference.HasThis ? "instance " : string.Empty;
                        builder.Append($"{instanceString}{methodReference.ReturnType.ToILType()} {methodReference.DeclaringType.ToILType()}::{methodReference.Name}{GetMethodCallParameters(methodReference)}");
                        break;
                    case FieldDefinition fieldDefinition:
                        builder.Append($"{fieldDefinition.FieldType.ToILType()} {fieldDefinition.DeclaringType.ToILType()}::{fieldDefinition.Name}");
                        break;
                    default:
                        builder.Append(instruction.Operand);
                        break;
                }
            }

            return builder.ToString();
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
                    builder.Append($"{parameterDefinition.ParameterType.ToILType()}");
                }
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}