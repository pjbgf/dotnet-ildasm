using System;
using System.Collections.Generic;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DotNet.Ildasm
{
    public static class InstructionExtensions
    {
        public static void WriteIL(this Instruction instruction, IOutputWriter outputWriter)
        {
            outputWriter.Write($"IL_{instruction.Offset:x4}: {instruction.OpCode.ToString()}");
            WriteOperandIL(instruction, outputWriter);
            outputWriter.Write(Environment.NewLine);
        }

        private static void WriteOperandIL(Instruction instruction, IOutputWriter writer)
        {
            if (instruction.Operand != null)
            {
                writer.Write(" ");

                if (instruction.OpCode.OperandType == OperandType.InlineString)
                {
                    writer.Write($"\"{instruction.Operand}\"");
                }
                else
                {
                    switch (instruction.Operand)
                    {
                        case Instruction subInstruction:
                            writer.Write($"IL_{subInstruction.Offset:x4}");
                            break;
                        case MethodReference methodReference:
                            var instanceString = methodReference.HasThis ? "instance " : string.Empty;
                            writer.Write(
                                $"{instanceString}{methodReference.ReturnType.ToIL()} class {methodReference.DeclaringType.ToIL()}::{methodReference.Name}");

                            WriteMethodCallParameters(methodReference, writer);
                            break;
                        case FieldDefinition fieldDefinition:
                            if (fieldDefinition.FieldType.IsGenericInstance || fieldDefinition.FieldType.MetadataType == MetadataType.Class)
                                writer.Write("class ");

                            writer.Write($"{fieldDefinition.FieldType.ToIL()} {fieldDefinition.DeclaringType.ToIL()}::{EscapeIfNeeded(fieldDefinition.Name)}");
                            break;
                        default:
                            var operandType = instruction.Operand.GetType();
                            if (operandType.IsClass)
                                writer.Write("class ");

                            writer.Write(instruction.Operand.ToString());
                            break;
                    }
                }
            }
        }

        private static void WriteMethodCallParameters(MethodReference method, IOutputWriter writer)
        {
            writer.Write("(");

            if (method.HasParameters)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");

                    var parameterDefinition = method.Parameters[i];
                    writer.Write($"{parameterDefinition.ParameterType.ToIL()}");
                }
            }

            writer.Write(")");
        }

        private static string EscapeIfNeeded(string fieldName)
        {
            if (fieldName.Contains("<"))
                return $"'{fieldName}'";

            return fieldName;
        }
    }
}