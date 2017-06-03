using System;
using System.Text;
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
                                $"{instanceString}{methodReference.ReturnType.ToIL()} {methodReference.DeclaringType.ToIL()}::{methodReference.Name}");
                            WriteMethodCallParameters(methodReference, writer);
                            break;
                        case FieldDefinition fieldDefinition:
                            //HACK: There must be another way to identify when single quotes are required.
                            if (fieldDefinition.Name.Contains("<"))
                                writer.Write(
                                    $"{fieldDefinition.FieldType.ToIL()} {fieldDefinition.DeclaringType.ToIL()}::'{fieldDefinition.Name}'");
                            else
                                writer.Write(
                                    $"{fieldDefinition.FieldType.ToIL()} {fieldDefinition.DeclaringType.ToIL()}::{fieldDefinition.Name}");
                            break;
                        default:
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



        public static void WriteILSignature(this MethodDefinition method, IOutputWriter writer)
        {
            writer.Write(".method");

            if (method.IsPublic)
                writer.Write(" public");
            else if (method.IsPrivate)
                writer.Write(" private");

            if (method.IsHideBySig)
                writer.Write(" hidebysig");

            if (method.IsNewSlot)
                writer.Write(" newslot");

            if (method.IsAbstract)
                writer.Write(" abstract");

            if (method.IsVirtual)
                writer.Write(" virtual");

            if (method.IsSpecialName)
                writer.Write(" specialname");

            if (method.IsRuntimeSpecialName)
                writer.Write(" rtspecialname");

            if (method.IsFinal)
                writer.Write(" final");

            if (!method.IsStatic)
                writer.Write(" instance");
            else
                writer.Write(" static");

            writer.Write($" {method.ReturnType.ToIL()}");
            writer.Write($" {method.Name}");

            WriteMethodSignatureParameters(method, writer);

            if (method.IsManaged)
                writer.Write(" cil managed");
        }

        private static void WriteMethodSignatureParameters(MethodDefinition method, IOutputWriter writer)
        {
            writer.Write("(");

            if (method.HasParameters)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");

                    var parameterDefinition = method.Parameters[i];
                    writer.Write($"{parameterDefinition.ParameterType.ToIL()} ");

                    if (parameterDefinition.Name == "value")
                        writer.Write($"'{parameterDefinition.Name}'");
                    else
                        writer.Write(parameterDefinition.Name);
                }
            }

            writer.Write(")");
        }

    }
}