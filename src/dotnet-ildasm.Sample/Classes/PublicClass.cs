using System;

namespace dotnet_ildasm.Sample.Classes
{
    public class PublicClass
    {
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
            if (parameter > 10)
            {
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
