using Castle.DynamicProxy.Generators;
using Mono.Cecil;
using Mono.Collections.Generic;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.SampleTests
{
    public class DisassemblerShould
    {
        private static readonly string DotnetIldasmSampleStandardDll = "dotnet-ildasm.Sample.Standard.dll";
        private IAssemblyDefinitionResolver _assemblyDefinitionResolver;
        private IAssemblyDataProcessor _assemblyProcessorMock;

        public DisassemblerShould()
        {
            _assemblyDefinitionResolver = Substitute.For<IAssemblyDefinitionResolver>();
            _assemblyProcessorMock = Substitute.For<IAssemblyDataProcessor>();
            
            var assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(DotnetIldasmSampleStandardDll);
            _assemblyDefinitionResolver.Resolve(Arg.Any<string>()).Returns(assemblyDefinition);
        }
        
        [Fact]
        public void Write_Assembly_ExternalReferences_then_AssemblySection()
        {
            var disassembler = new Disassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandOptions(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteAssemblyExternalReferences(Arg.Any<AssemblyDefinition>());
                _assemblyProcessorMock.WriteAssemblySection(Arg.Any<AssemblyDefinition>());
            });
        }
        
        [Fact]
        public void Write_AssemblySection_then_ModuleSection()
        {
            var disassembler = new Disassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandOptions(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteAssemblySection(Arg.Any<AssemblyDefinition>());
                _assemblyProcessorMock.WriteModuleSection(Arg.Any<ModuleDefinition>());
            });
        }
        
        [Fact]
        public void Write_ModuleSection_then_ModuleTypes()
        {
            var disassembler = new Disassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandOptions(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteModuleSection(Arg.Any<ModuleDefinition>());
                _assemblyProcessorMock.WriteModuleTypes(Arg.Any<Collection<TypeDefinition>>(), Arg.Any<ItemFilter>());
            });
        }
        
        [Fact]
        public void Write_Nothing_But_ModuleTypes_When_Filter_Is_Set()
        {
            var disassembler = new Disassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandOptions(), new ItemFilter("PublicClass::PublicVoidMethod"));

            _assemblyProcessorMock.Received(0).WriteAssemblyExternalReferences(Arg.Any<AssemblyDefinition>());
            _assemblyProcessorMock.Received(0).WriteAssemblySection(Arg.Any<AssemblyDefinition>());
            _assemblyProcessorMock.Received(0).WriteModuleSection(Arg.Any<ModuleDefinition>());
            _assemblyProcessorMock.Received(1).WriteModuleTypes(Arg.Any<Collection<TypeDefinition>>(), Arg.Any<ItemFilter>());
        }
    }
}