using System.Linq;
using Mono.Cecil.Cil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class InstructionProcessorShould
    {
        private readonly IOutputWriter _outputWriter;

        public InstructionProcessorShould()
        {
            _outputWriter = Substitute.For<IOutputWriter>();
        }

        [Fact]
        public void Be_Able_To_Write_Literal_String()
        {
            var instruction = Instruction.Create(OpCodes.Ldstr, "some string");
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: ldstr \"some string\"");
        }

        [Fact]
        public void Be_Able_To_Write_Literal_Integer32()
        {
            var instruction = Instruction.Create(OpCodes.Ldc_I4, 356);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: ldc.i4 356");
        }

        [Fact]
        public void Be_Able_To_Write_BackingField()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methoDefinition = type.Methods.First(x => x.Name == "get_Property1");
            var backingFieldReferenceInstruction = methoDefinition.Body.Instructions[1];
            var instructionProcessor = new InstructionProcessor(_outputWriter);
            
            instructionProcessor.WriteInstruction(backingFieldReferenceInstruction);

            _outputWriter.Received(1).WriteLine("IL_0001: ldfld string dotnet_ildasm.Sample.Classes.PublicClass::'<Property1>k__BackingField'");
        }

        [Fact]
        public void Be_Able_To_Write_IF_statement()
        {
            var instructionTarget = Instruction.Create(OpCodes.Nop);
            instructionTarget.Offset = 1;
            var instruction = Instruction.Create(OpCodes.Brfalse_S, instructionTarget);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: brfalse.s IL_0001");
        }

        [Fact]
        public void Be_Able_To_Write_Field_Reference()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicStruct");
            var fieldDefinition = type.Fields.First(x => x.Name == "X");
            var instruction = Instruction.Create(OpCodes.Stfld, fieldDefinition);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: stfld int32 dotnet_ildasm.Sample.Structs.PublicStruct::X");
        }

        [Fact]
        public void Be_Able_To_Write_Instance_Method_Call()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methoDefinition = type.Methods.First(x => x.Name == "PublicVoidMethodSingleParameter");
            var instruction = Instruction.Create(OpCodes.Call, methoDefinition);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: call instance void dotnet_ildasm.Sample.Classes.PublicClass::PublicVoidMethodSingleParameter(string)");
        }

        [Fact]
        public void Be_Able_To_Write_Instance_Property_Reference()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var propertyDefinition = type.Properties.First(x => x.Name == "Property1");
            var instruction = Instruction.Create(OpCodes.Call, propertyDefinition.GetMethod);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: call instance string dotnet_ildasm.Sample.Classes.PublicClass::get_Property1()");
        }

        [Fact]
        public void Be_Able_To_Write_Static_Method_Call()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "StaticClass");
            var methoDefinition = type.Methods.First(x => x.Name == "Method2");
            var instruction = Instruction.Create(OpCodes.Call, methoDefinition);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);
            
            _outputWriter.Received(1).WriteLine("IL_0000: call void dotnet_ildasm.Sample.Classes.StaticClass::Method2()");
        }
    }
}
