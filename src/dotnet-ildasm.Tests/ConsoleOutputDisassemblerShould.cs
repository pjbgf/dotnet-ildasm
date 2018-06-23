using System;
using DotNet.Ildasm.Configuration;
using Mono.Cecil;
using Mono.Collections.Generic;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class ConsoleOutputDisassemblerShould
    {
        private readonly IAssemblyDefinitionResolver _assemblyDefinitionResolver;
        private readonly IAssemblyDecompiler _assemblyProcessorMock;

        public ConsoleOutputDisassemblerShould()
        {
            _assemblyDefinitionResolver = Substitute.For<IAssemblyDefinitionResolver>();
            _assemblyProcessorMock = Substitute.For<IAssemblyDecompiler>();

            var assemblyDefinition = Mono.Cecil.AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition("test", Version.Parse("1.0.0")),
                "Module1", ModuleKind.Dll);

            _assemblyDefinitionResolver.Resolve(Arg.Any<string>()).Returns(assemblyDefinition);
        }

        [Fact]
        public void Not_Execute_If_Assembly_File_Does_Not_Exist()
        {
            var disassembler = new ConsoleOutputDisassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            _assemblyDefinitionResolver.Resolve(Arg.Any<string>()).Returns((AssemblyDefinition)null);

            ExecutionResult executionResult = disassembler.Execute(new CommandArgument(), new ItemFilter(string.Empty));

            Assert.False(executionResult.Succeeded);
            Assert.Equal("Error: Assembly could not be loaded, please check the path and try again.", executionResult.Message);
        }

        [Fact]
        public void Write_Assembly_ExternalReferences_then_AssemblySection()
        {
            var disassembler = new ConsoleOutputDisassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandArgument(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteAssemblyExternalReferences(Arg.Any<AssemblyDefinition>());
                _assemblyProcessorMock.WriteAssemblySection(Arg.Any<AssemblyDefinition>());
            });
        }
        
        [Fact]
        public void Write_AssemblySection_then_ModuleSection()
        {
            var disassembler = new ConsoleOutputDisassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandArgument(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteAssemblySection(Arg.Any<AssemblyDefinition>());
                _assemblyProcessorMock.WriteModuleSection(Arg.Any<ModuleDefinition>());
            });
        }
        
        [Fact]
        public void Write_ModuleSection_then_ModuleTypes()
        {
            var disassembler = new ConsoleOutputDisassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandArgument(), new ItemFilter(string.Empty));

            Received.InOrder(() =>
            {
                _assemblyProcessorMock.WriteModuleSection(Arg.Any<ModuleDefinition>());
                _assemblyProcessorMock.WriteModuleTypes(Arg.Any<Collection<TypeDefinition>>(), Arg.Any<ItemFilter>());
            });
        }

        [Fact]
        public void Write_Nothing_But_ModuleTypes_When_Filter_Is_Set()
        {
            var disassembler = new ConsoleOutputDisassembler(_assemblyProcessorMock, _assemblyDefinitionResolver);
            
            disassembler.Execute(new CommandArgument(), new ItemFilter("PublicClass::PublicVoidMethod"));

            _assemblyProcessorMock.Received(0).WriteAssemblyExternalReferences(Arg.Any<AssemblyDefinition>());
            _assemblyProcessorMock.Received(0).WriteAssemblySection(Arg.Any<AssemblyDefinition>());
            _assemblyProcessorMock.Received(0).WriteModuleSection(Arg.Any<ModuleDefinition>());
            _assemblyProcessorMock.Received(1).WriteModuleTypes(Arg.Any<Collection<TypeDefinition>>(), Arg.Any<ItemFilter>());
        }
    }
}