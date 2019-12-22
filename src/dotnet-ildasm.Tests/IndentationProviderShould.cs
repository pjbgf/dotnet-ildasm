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
        [InlineData(".assembly")]
        [InlineData(".module")]
        [InlineData(".class")]
        public void Double_Breakline_Before_Specific_Keywords(string inputIL)
        {
            var indentation = new AutoIndentOutputWriter(_outputWriterDouble);
            string expectedIL = $"{Environment.NewLine+Environment.NewLine}{inputIL}";

            indentation.Write(inputIL);
            var actualIL = _outputWriterDouble.ToString();

            Assert.Equal(expectedIL, actualIL);
        }

        [Theory]
        [InlineData(".method public")]
        [InlineData(".field")]
        [InlineData(".method")]
        [InlineData(".property")]
        [InlineData(".event")]
        public void Single_Breakline_Before_Specific_Keywords(string inputIL)
        {
            var indentation = new AutoIndentOutputWriter(_outputWriterDouble);
            string expectedIL = $"{Environment.NewLine}{inputIL}";

            indentation.Write(inputIL);
            var actualIL = _outputWriterDouble.ToString();

            Assert.Equal(expectedIL, actualIL);
        }

        [Fact]
        public void Add_No_Spaces_Outside_Of_Brackets()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Write("public static");
            var actualIL = _outputWriterDouble.ToString();

            Assert.Equal("public static", actualIL);
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
        public void Add_Two_Spaces_For_IL_Statements()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply(".method public {");
            autoIndentWriter.Apply("IL_0000: nop");

            _outputWriterMock.Received().Write("  IL_0000: nop");
        }

        [Fact]
        public void Add_Two_Spaces_For_catch_Statements()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply(".method public {");
            autoIndentWriter.Apply("catch");

            _outputWriterMock.Received().Write("  catch");
        }

        [Fact]
        public void Add_Single_LineBreak_After_Opening_Nested_Brackets()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterDouble);

            autoIndentWriter.Apply(".class ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".method public ");
            autoIndentWriter.Apply("{");
            autoIndentWriter.Apply(".maxstack 8");
            
            var actualIL = _outputWriterDouble.ToString();
            var expectedIL = $"{Environment.NewLine+Environment.NewLine}.class {{{Environment.NewLine}  .method public   {{    .maxstack 8";

            Assert.Equal(expectedIL, actualIL);
        }

        [Fact]
        public void Not_Remove_Spaces_On_Brackets_Closing_Line()
        {
            var autoIndentWriter = new AutoIndentOutputWriter(_outputWriterMock);

            autoIndentWriter.Apply(".class public {");
            autoIndentWriter.Apply(".method public {");
            autoIndentWriter.Apply("}");
            
            _outputWriterMock.Received().Write("  }");
        }

        [Fact]
        public void Remove_Spaces_After_Brackets_Are_Closed()
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