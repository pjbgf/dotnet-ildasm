using Mono.Cecil.Cil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.SampleTests
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
            var instruction = Instruction.Create(OpCodes.Ldc_I4_S, 356);
            var instructionProcessor = new InstructionProcessor(_outputWriter);

            instructionProcessor.WriteInstruction(instruction);

            _outputWriter.Received(1).WriteLine("IL_0000: ldc.i4 356");
        }
    }
}
