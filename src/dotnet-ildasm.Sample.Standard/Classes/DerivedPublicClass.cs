using System;

namespace dotnet_ildasm.Sample.Standard.Classes
{
    public class DerivedPublicClass : PublicAbstractClass
    {
        public override void PublicAbstractMethod()
        {
        }

        public sealed override void PublicAbstractSealedMethod()
        {
        }

        public new void PublicImplementedMethod()
        {
        }
    }
}