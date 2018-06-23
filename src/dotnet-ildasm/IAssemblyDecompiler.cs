using System;
using DotNet.Ildasm.Configuration;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace DotNet.Ildasm
{
    public interface IAssemblyDecompiler : IDisposable
    {
        void WriteAssemblyExternalReferences(AssemblyDefinition assembly);
        
        void WriteAssemblySection(AssemblyDefinition assembly);
        
        void WriteModuleSection(ModuleDefinition module);
    
        void WriteModuleTypes(Collection<TypeDefinition> types, ItemFilter itemFilter);
    }
}