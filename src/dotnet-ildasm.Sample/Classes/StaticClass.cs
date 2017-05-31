using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_ildasm.Sample.Classes
{
    public static class StaticClass
    {
        public static void Method1()
        {
            Method2();
        }
        public static void Method2()
        {
        }
    }
}
