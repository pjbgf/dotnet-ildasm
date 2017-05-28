namespace dotnet_ildasm.Sample.Structs
{
    public struct PublicStruct
    {
        public PublicStruct(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X;
        public int Y;
    }
}