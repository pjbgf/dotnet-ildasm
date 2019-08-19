using System;
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
        [InlineData(".method public")]
        [InlineData(".assembly")]
        [InlineData(".module")]
        [InlineData(".class")]
        public void Breakline_Before_Specific_Keywords(string inputIL)
        {
            var indentation = new AutoIndentOutputWriter(_outputWriterDouble);
            string expectedIL = $"{Environment.NewLine}{inputIL}";

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
            
            var actualIL = _outputWriterDouble.ToString();
            var expectedIL = $"{Environment.NewLine}.class {{{Environment.NewLine}  .method public   {{    .maxstack 8";

            Assert.Equal(expectedIL, actualIL);
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

            var actualIL = _outputWriterDouble.ToString();
            var expectedIL = $"{{{Environment.NewLine}  .method public hidebysig ";

            Assert.Equal(expectedIL, actualIL);
        }
    }
}