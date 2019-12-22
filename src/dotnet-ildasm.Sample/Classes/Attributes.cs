using System;
using System.Diagnostics;

namespace dotnet_ildasm.Sample.Classes
{
    [AttributeUsage(AttributeTargets.All)]
    public class SomeAttribute : System.Attribute { }

    [DebuggerDisplayAttribute("Level=Class")]
    public class SomeClassWithAttribute
    {
        [DebuggerDisplayAttribute("Level=Field")]
        public readonly string SomeFieldWithAttribute = "Something 2";

        [Obsolete("This method should not be used...", false)]
        public void SomeMethodWithAttribute()
        {
        }

        [SomeAttribute]
        [DebuggerDisplayAttribute("Level=Property")]
        public string SomePropertyWithAttribute { get; set; }
    }

    [DebuggerDisplayAttribute("Level=Struct")]
    public class SomeStructWithAttribute
    {
    }
}
