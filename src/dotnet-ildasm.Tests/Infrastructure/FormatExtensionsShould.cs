using DotNet.Ildasm.Infrastructure;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class FormatExtensionsShould
    {
        [Theory]
        [InlineData(ushort.MinValue, "0x0000")]
        [InlineData(100, "0x0064")]
        [InlineData(ushort.MaxValue, "0xffff")]
        public void Format_UnsignedInt16_Into_Hexadecimal_string(ushort value, string expectedString)
        {
            var actual = value.ToHexadecimal();

            Assert.Equal(expectedString, actual);
        }

        [Theory]
        [InlineData(int.MinValue, "0x80000000")]
        [InlineData(100, "0x00000064")]
        [InlineData(int.MaxValue, "0x7fffffff")]
        public void Format_Int32_Into_Hexadecimal_string(int value, string expectedString)
        {
            var actual = value.ToHexadecimal();

            Assert.Equal(expectedString, actual);
        }

        [Theory]
        [InlineData(0UL, "0x00000000")]
        [InlineData(100, "0x00000064")]
        [InlineData(4294967295UL, "0xffffffff")]
        public void Format_UnsignedLong_Into_Hexadecimal_string(ulong value, string expectedString)
        {
            var actual = value.ToHexadecimal();

            Assert.Equal(expectedString, actual);
        }

        [Theory]
        [InlineData(new byte[] { 0, 255, 125 }, "00 FF 7D")]
        [InlineData(null, "")]
        [InlineData(new byte[]{}, "")]
        public void Format_ByteArray_Into_Hexadecimal_string(byte[] value, string expectedString)
        {
            var actual = value.ToHexadecimal();

            Assert.Equal(expectedString, actual);
        }
    }
}
