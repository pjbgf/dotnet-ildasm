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

        [SomeAttribute]
        [DebuggerDisplayAttribute("Level=Property")]
        public static string SomeStaticPropertyWithAttribute { get; set; }

        protected virtual void OnSomeEventWithAttribute(object e)
        {
            EventHandler<object> handler = SomeEventWithAttribute;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [SomeAttribute]
        public event EventHandler<object> SomeEventWithAttribute;
    }

    [DebuggerDisplayAttribute("Level=Struct")]
    public class SomeStructWithAttribute
    {
    }
}
