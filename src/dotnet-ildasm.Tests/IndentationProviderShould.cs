using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class IndentationProviderShould
    {
        [Fact]
        public void Add_No_Spaces_Outside_Of_Brackets()
        {
            var indentation = new IndentationProvider();

            var actual = indentation.Apply(".method public");

            Assert.Equal(".method public", actual);
        }
        
        [Fact]
        public void Apply_Indentation_After_Bracket_Is_Open()
        {
            var indentation = new IndentationProvider();

            var actual = indentation.Apply(".method public {");

            Assert.Equal(".method public {", actual);
        }
        
        [Fact]
        public void Remove_Indentation_In_Same_Line_When_Closing_Bracket()
        {
            var indentation = new IndentationProvider();

            indentation.Apply(".method public {");
            indentation.Apply(".maxstack 8");
            var actual = indentation.Apply("}");

            Assert.Equal("}", actual);
        }
        
        [Fact]
        public void Add_Two_Spaces_Within_First_Open_Brackets()
        {
            var indentation = new IndentationProvider();

            indentation.Apply(".method public {");
            var actual = indentation.Apply(".maxstack 8");

            Assert.Equal("  .maxstack 8", actual);
        }
        
        [Fact]
        public void Remove_Spaces_Once_Brackets_Are_Closed()
        {
            var indentation = new IndentationProvider();

            indentation.Apply(".method public {");
            indentation.Apply(".maxstack 8");
            indentation.Apply("}");
            var actual = indentation.Apply(".method private");

            Assert.Equal(".method private", actual);
        }
        
        [Fact]
        public void Ignore_Orphan_Closing_Brackets()
        {
            var indentation = new IndentationProvider();

            indentation.Apply("{}}");
            indentation.Apply(".method private {");
            var actual = indentation.Apply(".maxstack 8");

            Assert.Equal("  .maxstack 8", actual);
        }
    }
}