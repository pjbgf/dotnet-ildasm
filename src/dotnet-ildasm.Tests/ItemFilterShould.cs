using System;
using Xunit;
using DotNet.Ildasm;

namespace DotNet.Ildasm.Tests
{
    public class ItemFilterShould
    {
        [Fact]
        public void Parse_ClassName_And_MethodName_Separated_By_DoubleColon()
        {
            string itemFilter = "Program::.ctor";
            var filterParser = new ItemFilter(itemFilter);

            Assert.Equal("Program", filterParser.Class);
            Assert.Equal(".ctor", filterParser.Method);
        }

        [Fact]
        public void Parse_ClassName_By_Itself()
        {
            string itemFilter = "Program";
            var filterParser = new ItemFilter(itemFilter);

            Assert.Equal("Program", filterParser.Class);
            Assert.Null(filterParser.Method);
        }

        [Fact]
        public void Parse_MethodName_By_Itself()
        {
            string itemFilter = "::Main";
            var filterParser = new ItemFilter(itemFilter);

            Assert.Null(filterParser.Class);
            Assert.Equal("Main", filterParser.Method);
        }

        [Fact]
        public void Ignore_Empty_Filter()
        {
            string itemFilter = String.Empty;
            var filterParser = new ItemFilter(itemFilter);
            
            Assert.Null(filterParser.Class);
            Assert.Null(filterParser.Method);
        }

        [Fact]
        public void Known_When_No_Filter_Is_Set()
        {
            string itemFilter = String.Empty;
            var filterParser = new ItemFilter(itemFilter);

            Assert.False(filterParser.HasFilter);
        }

        [Fact]
        public void Known_When_Class_Filter_Is_Set()
        {
            string itemFilter = "Program";
            var filterParser = new ItemFilter(itemFilter);

            Assert.True(filterParser.HasFilter);
        }

        [Fact]
        public void Known_When_Method_Filter_Is_Set()
        {
            string itemFilter = "::.ctor";
            var filterParser = new ItemFilter(itemFilter);

            Assert.True(filterParser.HasFilter);
        }

        [Fact]
        public void Known_When_Both_Filters_Are_Set()
        {
            string itemFilter = "Program::.ctor";
            var filterParser = new ItemFilter(itemFilter);

            Assert.True(filterParser.HasFilter);
        }
    }
}