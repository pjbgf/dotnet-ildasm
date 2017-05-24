namespace DotNet.Ildasm
{
    interface IOutputWriter
    {
        void Write(string value);
        void WriteLine(string value = "");
    }
}