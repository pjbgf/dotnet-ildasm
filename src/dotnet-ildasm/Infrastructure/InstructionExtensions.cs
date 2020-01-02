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
                            if (methodReference.CallingConvention == MethodCallingConvention.Generic)
                            {
                                var genericInstance = instruction.Operand as GenericInstanceMethod;
                                if (genericInstance != null)
                                {
                                    writer.Write(
                                        $"{instanceString}{genericInstance.ReturnType.ToString()} class {genericInstance.DeclaringType.ToIL()}::{genericInstance.Name}");

                                    WriteGenericMethodCallParameters(genericInstance, writer);
                                }
                            }
                            else
                            {
                                writer.Write(
                                    $"{instanceString}{methodReference.ReturnType.ToIL()} class {methodReference.DeclaringType.ToIL()}::{methodReference.Name}");

                                WriteMethodCallParameters(methodReference, writer);
                            }

                            break;
                        case FieldDefinition fieldDefinition:
                            if (fieldDefinition.FieldType.IsGenericInstance || fieldDefinition.FieldType.MetadataType == MetadataType.Class)
                                writer.Write("class ");

                            writer.Write($"{fieldDefinition.FieldType.ToIL()} {fieldDefinition.DeclaringType.ToIL()}::{EscapeIfNeeded(fieldDefinition.Name)}");
                            break;
                        default:
                            var operandType = instruction.Operand.GetType();
                            if (operandType.IsClass)
                            {
                                writer.Write($"class ");

                                var genericInstance = instruction.Operand as GenericInstanceType;
                                if (genericInstance != null)
                                {
                                    writer.Write($"[{genericInstance.Scope.Name}]");
                                }
                            }

                            writer.Write(instruction.Operand.ToString());
                            break;
                    }
                }
            }
        }


        private static void WriteGenericMethodCallParameters(GenericInstanceMethod method, IOutputWriter writer)
        {
            // call !!0 class [mscorlib]System.Threading.Interlocked::CompareExchange<class [mscorlib]System.EventHandler`1<object>> ([out] !!0&, !!0, !!0)
            writer.Write("<");
            var argI = 0;
            foreach (var arg in method.GenericArguments)
            {
                if (argI > 0)
                    writer.Write(", ");

                var argType = arg.GetElementType();
                if (argType.MetadataType == MetadataType.Class)
                    writer.Write("class ");

                writer.Write(argType.ToIL());

                var genericArg = arg as GenericInstanceType;
                if (genericArg != null)
                {
                    writer.Write("<");

                    var subArgI = 0;
                    foreach (var subArg in genericArg.GenericArguments)
                    {
                        if (subArgI > 0)
                            writer.Write(", ");

                        writer.Write(subArg.GetElementType().ToIL());
                    }

                    writer.Write(">");
                }
            }
            writer.Write(">");
            writer.Write("(");

            for (int i = 0; i < method.Parameters.Count; i++)
            {
                if (i > 0)
                    writer.Write(", ");

                var parameter = method.Parameters[i];
                if (parameter.IsOut)
                    writer.Write("[out] ");

                writer.Write($"{parameter.ParameterType.FullName}");
            }

            writer.Write(")");
        }

        private static void WriteMethodCallParameters(MethodReference method, IOutputWriter writer)
        {
            writer.Write("(");

            if (method.HasParameters)
            {
                var genericIndex = 0;
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");

                    var parameterDefinition = method.Parameters[i];

                    if (method.ContainsGenericParameter && i % 2 != 0)
                        writer.Write($"!{genericIndex++}");
                    else
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