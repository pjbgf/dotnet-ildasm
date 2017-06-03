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

        [Theory]
        [InlineData("PublicClass", "PublicVoidMethod", ".method public hidebysig instance void PublicVoidMethod() cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodSingleParameter", ".method public hidebysig instance void PublicVoidMethodSingleParameter(string parameter1) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodTwoParameters", ".method public hidebysig instance void PublicVoidMethodTwoParameters(string parameter1, int32 parameter2) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodParams", ".method public hidebysig instance void PublicVoidMethodParams(string[] parameters) cil managed")]
        [InlineData("PublicClass", "set_Property1", ".method public hidebysig specialname instance void set_Property1(string 'value') cil managed")]
        [InlineData("PublicAbstractClass", "PublicAbstractMethod", ".method public hidebysig newslot abstract virtual instance void PublicAbstractMethod() cil managed")]
        [InlineData("PublicAbstractClass", "PublicImplementedMethod", ".method public hidebysig instance void PublicImplementedMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicAbstractMethod", ".method public hidebysig virtual instance void PublicAbstractMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicAbstractSealedMethod", ".method public hidebysig virtual final instance void PublicAbstractSealedMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicImplementedMethod", ".method public hidebysig instance void PublicImplementedMethod() cil managed")]
        public void Write_Method_Signature(string className, string methodName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var method = type.Methods.FirstOrDefault(x => x.Name == methodName);
            var methodProcessor = new MethodProcessor(_outputWriterMock);

            method.WriteILSignature(_outputWriter);

            Assert.Equal(expectedIL, _outputWriter.ToString());
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
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methoDefinition = type.Methods.First(x => x.Name == "get_Property1");
            var backingFieldReferenceInstruction = methoDefinition.Body.Instructions[1];

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
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methoDefinition = type.Methods.First(x => x.Name == "PublicVoidMethodSingleParameter");
            var instruction = Instruction.Create(OpCodes.Call, methoDefinition);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call instance void dotnet_ildasm.Sample.Classes.PublicClass::PublicVoidMethodSingleParameter(string)", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Instance_Property_Reference()
        {
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "PublicClass");
            var propertyDefinition = type.Properties.First(x => x.Name == "Property1");
            var instruction = Instruction.Create(OpCodes.Call, propertyDefinition.GetMethod);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call instance string dotnet_ildasm.Sample.Classes.PublicClass::get_Property1()", _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Write_Static_Method_Call()
        {
            var type = _assemblyDefinition.Modules.First().Types.First(x => x.Name == "StaticClass");
            var methoDefinition = type.Methods.First(x => x.Name == "Method2");
            var instruction = Instruction.Create(OpCodes.Call, methoDefinition);

            instruction.WriteIL(_outputWriter);

            Assert.Equal("IL_0000: call void dotnet_ildasm.Sample.Classes.StaticClass::Method2()", _outputWriter.ToString());
        }
    }
}
