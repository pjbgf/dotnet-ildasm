namespace DotNet.Ildasm
{
    public static class FormatHelper
    {
        public static string GetHexadecimal(ushort value)
        {
            return value.ToString("x4");
        }

        public static string GetHexadecimal(int value)
        {
            return value.ToString("x8");
        }

        public static string GetHexadecimal(ulong value)
        {
            return value.ToString("x8");
        }
    }
}
