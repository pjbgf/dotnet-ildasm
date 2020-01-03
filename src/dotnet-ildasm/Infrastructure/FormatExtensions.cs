using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Ildasm.Infrastructure
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
            if (value == null)
                return string.Empty;

            return BitConverter.ToString(value).Replace('-', ' ');
        }

        public static string ToStringValue(this byte[] value)
        {
            if (value == null)
                return string.Empty;

            var encoder = ASCIIEncoding.GetEncoding(0, 
                                  new EncoderReplacementFallback("."),
                                  new DecoderReplacementFallback("."));

            var decoded = encoder.GetString(value);
            return Regex.Replace(decoded, "\\W", ".");
        }
        
    }
}
