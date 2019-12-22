using System;
using System.Diagnostics;

namespace dotnet_ildasm.Sample.Classes
{
    [AttributeUsage(AttributeTargets.All)]
    public class SomeAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class AnotherAttribute : System.Attribute { }

    [DebuggerDisplayAttribute("Level=Class")]
    public class SomeClassWithAttribute
    {
        [SomeAttribute]
        public SomeClassWithAttribute()
        {

        }

        [DebuggerDisplayAttribute("Level=Field")]
        public readonly string SomeFieldWithAttribute = "Something 2";

        [Obsolete("This method should not be used...", false)]
        public void SomeMethodWithAttribute()
        {
        }

        [return: SomeAttribute, Another()]
        public bool SomeMethodWithAttributeOnReturnValue()
        {
            return true;
        }

        public bool SomeMethodWithAttributeOnParameter([SomeAttribute]string arg1, [AnotherAttribute]bool arg2)
        {
            return true;
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

        protected virtual void OnSomeStaticEventWithAttribute(string e)
        {
            EventHandler<string> handler = SomeStaticEventWithAttribute;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [SomeAttribute]
        public event EventHandler<object> SomeEventWithAttribute;

        [SomeAttribute]
        public static event EventHandler<string> SomeStaticEventWithAttribute;
    }

    [DebuggerDisplayAttribute("Level=Struct")]
    public class SomeStructWithAttribute
    {
    }

    [SomeAttribute]
    public enum SomeEnumWithAttribute : int
    {
        ItemWithoutAttribute = 0,

        [SomeAttribute]
        ItemWithAttribute = 1
    }

    [SomeAttribute]
    public interface SomeInterfaceWithAttribute
    {

    }
}
