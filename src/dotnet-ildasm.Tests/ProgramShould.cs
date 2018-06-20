﻿using System;
using DotNet.Ildasm.Configuration;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class ProgramShould
    {
        [Fact]
        public void Abort_If_No_Parameters_Are_Sent()
        {
            var returnCode = Program.Main(new string[0]);

            Assert.Equal(-1, returnCode);
        }
    }
}