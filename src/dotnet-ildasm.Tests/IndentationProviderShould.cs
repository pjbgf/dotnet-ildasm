using DotNet.Ildasm.Tests.Internal;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class IndentationProviderShould
    {
        private readonly OutputWriterDouble _outputWriterDouble;
        private readonly IOutputWriter _outputWriterMock;

        public IndentationProviderShould()
        {
            _outputWriterDouble = new OutputWriterDouble();
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Theory]
        [InlineData(".method public", "\r\n.method public")]
        [InlineData(".assembly", "\r\n.assembly")]
        [InlineData(".module", "\r\n.module")]
        [InlineData(".class", "\r\n.class")]
        public void Breakline_Before_Specific_Keywords(string inputIL, string expectedIL)
        {
            var indentation = new AutoIndentOutputWriter(_outputWriterDouble);

            indentation.Write(inputIL);

            Assert.Equal(expectedIL, _outputWriterDouble.ToString());
        }

        [Fact]
        public void Add_No_Spaces_Outside_Of_Brackets()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Write("public static");

            Assert.Equal("public static", _outputWriterDouble.ToString());
        }

        [Fact]
        public void Add_Two_Spaces_Within_First_Open_Brackets()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply(".method public {");
            autoIndentWriter.Apply(".maxstack 8");

            _outputWriterMock.Received().Write("  .maxstack 8");
        }

        [Fact]
        public void Add_Two_Spaces_In_Same_Line_As_Second_Brackets_Opens()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Apply(".class ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".method public ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".maxstack 8");

            Assert.Equal("\r\n.class {\r\n  .method public   {    .maxstack 8", _outputWriterDouble.ToString());
        }

        [Fact]
        public void Keep_Two_Spaces_In_Same_Line_As_Second_Brackets_Closes()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Apply(".class ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".method public ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".maxstack 8");

            Assert.Equal("\r\n.class {\r\n  .method public   {    .maxstack 8", _outputWriterDouble.ToString());
        }

        [Fact]
        public void Remove_Spaces_Once_Brackets_Are_Closed()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply(".method public {");
            autoIndentWriter.Apply(".maxstack 8");
            autoIndentWriter.Apply("}");
            autoIndentWriter.Apply(".method private");
            
            _outputWriterMock.Received().Write(".method private");
        }

        [Fact]
        public void Ignore_Orphan_Closing_Brackets()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply("{}}");
            autoIndentWriter.Apply(".method private {");
            autoIndentWriter.Apply(".maxstack 8");
            
            _outputWriterMock.Received().Write("  .maxstack 8");
        }

        [Fact]
        public void Not_Apply_Indentation_In_Between_Signature_Keywords()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".method ");
            autoIndentWriter.Apply("public ");
            autoIndentWriter.Apply("hidebysig ");

            Assert.Equal("{\r\n  .method public hidebysig ", _outputWriterDouble.ToString());
        }
    }
}