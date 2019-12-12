using System;
using System.Diagnostics;

namespace dotnet_ildasm.Sample.Classes
{
    [DebuggerDisplayAttribute("Level=Class")]
    public class SomeClassWithAttribute
    {
        [DebuggerDisplayAttribute("Level=Field")]
        public readonly string SomeFieldWithAttribute = "Something 2";

        [Obsolete("This method should not be used...", false)]
        public void SomeMethodWithAttribute()
        {
        }

        [DebuggerDisplayAttribute("Level=Property")]
        public string SomePropertyWithAttribute { get; set; }
    }

    [DebuggerDisplayAttribute("Level=Struct")]
    public class SomeStructWithAttribute
    {
    }
}
