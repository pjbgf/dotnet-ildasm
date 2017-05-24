
.assembly extern System.Runtime
{
// .publickeytoken 10110000 111111 1011111 1111111 10001 11010101 1010 111010 // Needs proper formatting
.ver 4:1:0:0
}

.assembly extern System.Console
{
// .publickeytoken 10110000 111111 1011111 1111111 10001 11010101 1010 111010 // Needs proper formatting
.ver 4:0:0:0
}

.assembly 'dotnet-hello'
{
.custom instance System.Void System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(System.Int32)
.custom instance System.Void System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor()
// The following custom attribute is added automatically for debugging purposes, do not uncomment. 
//.custom instance System.Void System.Diagnostics.DebuggableAttribute::.ctor(System.Diagnostics.DebuggableAttribute/DebuggingModes)
.custom instance System.Void System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyCompanyAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyConfigurationAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyDescriptionAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyFileVersionAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyInformationalVersionAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyProductAttribute::.ctor(System.String)
.custom instance System.Void System.Reflection.AssemblyTitleAttribute::.ctor(System.String)
.hash algorithm 0x00008004
.ver 4:3:0:0
}

.module 'dotnet-hello'
// MVID: {9e6c6dc5-7a98-4556-90eb-c37b505e5c86}
// .imagebase 0x000000 (Currently not supported)
// .file alignment 0x000000 (Currently not supported)
// .stackreserve 0x000000 (Currently not supported)
//.subsystem 0x003
// .cornflags 0x000000 (Currently not supported)
// image base:  

.class private auto ansi beforefieldinit dotnet_hello.Program extends System.Object{

.method private hidebysig System.Void dotnet_hello.Program::Main(System.String[]) cil managed{
.entrypoint
// Code size 13
.maxstack 8
IL_0000: nop
IL_0001: ldstr "Hello World!"
IL_0006: call System.Void System.Console::WriteLine(System.String)
IL_000b: nop
IL_000c: ret
}// End of method System.Void dotnet_hello.Program::Main(System.String[])

.method public hidebysig specialname rtspecialname instance System.Void dotnet_hello.Program::.ctor() cil managed{
// Code size 8
.maxstack 8
IL_0000: ldarg.0
IL_0001: call System.Void System.Object::.ctor()
IL_0006: nop
IL_0007: ret
}// End of method System.Void dotnet_hello.Program::.ctor()

} // End of class dotnet_hello.Program
