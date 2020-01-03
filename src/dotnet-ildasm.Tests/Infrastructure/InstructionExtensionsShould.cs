using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class InstructionExtensionsShould
    {
        private readonly OutputWriterDouble _outputWriter;
        private readonly IOutputWriter _outputWriterMock;
        private readonly AssemblyDefinition _assemblyDefinition;

        public InstructionExtensionsShould()
        {
            _outputWriter = new OutputWriterDouble();
            _outputWriterMock = Substitute.For<IOutputWriter>();
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
        }

        [Fact]
        public void Be_Able_To_Write_Literal_String()
        {
            var instruction = Instruction.Create(OpCodes.Ldstr, "some string");

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: ldstr \"some string\"", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Literal_Integer32()
        {
            var instruction = Instruction.Create(OpCodes.Ldc_I4, 356);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: ldc.i4 356", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_BackingField()
        {
            var method = GetMethod("PublicClass", "get_Property1");
            var backingFieldReferenceInstruction = method.Body.Instructions[1];

            backingFieldReferenceInstruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0001: ldfld string dotnet_ildasm.Sample.Classes.PublicClass::'<Property1>k__BackingField'", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_IF_statement()
        {
            var instructionTarget = Instruction.Create(OpCodes.Nop);
            instructionTarget.Offset = 1;
            var instruction = Instruction.Create(OpCodes.Brfalse_S, instructionTarget);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: brfalse.s IL_0001", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Field_Reference()
        {
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "PublicStruct");
            var fieldDefinition = type.Fields.First(x => x.Name == "X");
            var instruction = Instruction.Create(OpCodes.Stfld, fieldDefinition);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: stfld int32 dotnet_ildasm.Sample.Structs.PublicStruct::X", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Instance_Method_Call()
        {
            var method = GetMethod("PublicClass", "PublicVoidMethodSingleParameter");
            var instruction = Instruction.Create(OpCodes.Call, method);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call instance void class dotnet_ildasm.Sample.Classes.PublicClass::PublicVoidMethodSingleParameter(string)", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Instance_Property_Reference()
        {
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "PublicClass");
            var propertyDefinition = type.Properties.First(x => x.Name == "Property1");
            var instruction = Instruction.Create(OpCodes.Call, propertyDefinition.GetMethod);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call instance string class dotnet_ildasm.Sample.Classes.PublicClass::get_Property1()", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Static_Method_Call()
        {
            var method = GetMethod("StaticClass", "Method2");
            var instruction = Instruction.Create(OpCodes.Call, method);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call void class dotnet_ildasm.Sample.Classes.StaticClass::Method2()", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Call_To_Generic_Method()
        {
            var method = GetMethod("SomeClassWithAttribute", "OnSomeEventWithAttribute");
            Instruction callVirt = GetInstruction(method, OpCodes.Callvirt);

            callVirt.WriteIL(_outputWriter);

            Assert.Contains("callvirt instance void class [netstandard]System.EventHandler`1<System.Object>::Invoke([netstandard]System.Object, !0)", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_castclass_For_Generic_Type()
        {
            var method = GetMethod("SomeClassWithAttribute", "add_SomeEventWithAttribute");
            var castClass = GetInstruction(method, OpCodes.Castclass);

            castClass.WriteIL(_outputWriter);

            Assert.Equal("IL_0010: castclass class [netstandard]System.EventHandler`1<System.Object>", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Refer_To_Native_Int()
        {
            var method = GetMethod("StaticClass", "Method3");
            var castClass = GetInstruction(method, OpCodes.Ldsfld);

            castClass.WriteIL(_outputWriter);

            Assert.Contains("ldsfld native int [netstandard]System.IntPtr::Zero", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Generic_Method_Call()
        {
            var method = GetMethod("SomeClassWithAttribute", "add_SomeEventWithAttribute");
            var genericCall = method.Body.Instructions[14];

            genericCall.WriteIL(_outputWriter);

            Assert.Equal("IL_001e: call !!0 class [netstandard]System.Threading.Interlocked::CompareExchange<class [netstandard]System.EventHandler`1<[netstandard]System.Object>>(!!0&, !!0, !!0)", _outputWriter.ToString());
        }

        MethodDefinition GetMethod(string className, string methodName)
        {
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == className);
            return type.Methods.First(x => x.Name == methodName);
        }

        static Instruction GetInstruction(MethodDefinition method, OpCode opCode)
        {
            Instruction instruction = null;
            foreach (var inst in method.Body.Instructions)
            {
                if (inst.OpCode == opCode)
                {
                    instruction = inst;
                    break;
                }
            }

            return instruction;
        }
    }
}
