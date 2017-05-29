using System;
using System.Linq;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.SampleTests
{
    public class AssemblySectionProcessorShould
    {
        private static readonly string DotnetIldasmSampleStandardDll = "dotnet-ildasm.Sample.dll";
        private readonly IOutputWriter _outputWriterMock;
        private AssemblyDefinition _assemblyDefinition;

        public AssemblySectionProcessorShould()
        {
            _outputWriterMock = Substitute.For<IOutputWriter>();
            _assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(DotnetIldasmSampleStandardDll);
        }
        
        

//        [Fact]
//        public void Extract_CustomAttribute()
//        {
//            var expected = ".custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 )";
//            var customAttribute = _assemblyDefinition.CustomAttributes.First(x => string.Compare(x.AttributeType.Name, 
//                                                                                      "CompilationRelaxationsAttribute", 
//                                                                                      StringComparison.CurrentCultureIgnoreCase) == 0);
            
////            var actual = _cilHelper.GetCustomAttribute(customAttribute);
////
////            methodProcessor.GetCustomAttribute(method);
////
////            _outputWriterMock.Received(1).WriteLine(expectedIL);
//        }
    }
}