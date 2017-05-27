namespace DotNet.Ildasm
{
    public static class FormatExtensions
    {
        public static string ToHexadecimal(this ushort value)
        {
            return value.ToString("x4");
        }

        public static string ToHexadecimal(this int value)
        {
            return value.ToString("x8");
        }

        public static string ToHexadecimal(this ulong value)
        {
            return value.ToString("x8");
        }
    }
}
