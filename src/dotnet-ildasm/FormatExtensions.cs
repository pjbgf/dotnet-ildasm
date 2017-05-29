using System;

namespace DotNet.Ildasm
{
    public static class FormatExtensions
    {
        public static string ToHexadecimal(this ushort value)
        {
            return $"0x{value:x4}";
        }

        public static string ToHexadecimal(this int value)
        {
            return $"0x{value:x8}";
        }

        public static string ToHexadecimal(this ulong value)
        {
            return $"0x{value:x8}";
        }
        
        public static string ToHexadecimal(this byte[] value)
        {
            return BitConverter.ToString(value).Replace('-', ' ');
        }
    }
}
