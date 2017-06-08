using System;

namespace dotnet_ildasm.Sample.Classes
{
    public class PublicClass
    {
        public static readonly string StaticReadonlyField = "Something Static";
        public readonly string ReadonlyField = "Something 2";
        public readonly string Field = "Something 2";

        public void PublicVoidMethod()
        {
            PublicVoidMethodSingleParameter(null);
        }
        
        public void PublicVoidMethodSingleParameter(string parameter1)
        {
        }
        
        public void PublicVoidMethodTwoParameters(string parameter1, int parameter2)
        {
        }

        public void PublicVoidMethodParams(params string[] parameters)
        {
        }

        public void UsingIF(int parameter)
        {
            int localVariable = 5; 
            if (parameter > 10)
            {
                UsingTryCatch(localVariable);
            }
            else
            {
                UsingTryCatch(localVariable * parameter);
            }
        }

        public void UsingTryCatch(int parameter)
        {
            try
            {
                Console.WriteLine(parameter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string Property1 { get; set; }
    }
}
