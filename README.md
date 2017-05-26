# IL Disassembler  
[![Build status](https://ci.appveyor.com/api/projects/status/is3qcb0gnf018vx6/branch/master?svg=true)](https://ci.appveyor.com/project/pjbgf/dotnet-ildasm/branch/master) 
[![Nuget](https://img.shields.io/nuget/dt/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Nuget](https://img.shields.io/nuget/v/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Nuget](https://img.shields.io/nuget/vpre/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm)


# Install

1. Install dotnet-ildasm nuget in your project.
2. Add the following entry to the .csproj of your test project:

```
<ItemGroup>
  <DotNetCliToolReference Include="dotnet-ildasm" Version="*" />
</ItemGroup>
```
PS: Please specify the actual version instead of using *.


# Usage
Implemented as a dotnet cli extension, which allows for an easy to use command line.

Default options, generating a file named after the assembly (myassembly.il):
```
dotnet ildasm myassembly.dll
```

Output results to the command line:
```
dotnet ildasm myassembly.dll -t
```

Select specific method or classes to be disassembled:
```
dotnet ildasm myassembly.dll -t -i ClassName::Method
```

Defining a file name to output: 
```
dotnet ildasm myassembly.dll -o disassembledAssembly.il
```


# Status

* Some PE Headers are currently not being loaded (i.e. imagebase, file alignment, stackreserve, cornflags).
* Custom Attributes not fully disassembled.