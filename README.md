#IL Disassembler tool

# Usage
Implemented as a dotnet cli extension, which allows for an easy to use command line:

```
dotnet ildasm myassembly.dll /t
```

/t = Output as text
/o = Output path - Default is the name of assembly with the extension ".il"

# Install

1. Install dotnet-ildasm nuget in your project.
2. Add the following entry to the .csproj of your test project:

```
<ItemGroup>
  <DotNetCliToolReference Include="CoreCover" Version="*" />
</ItemGroup>
```
PS: Please specify the actual version instead of using *.

# Status

* Some PE Headers are currently not being loaded (i.e. imagebase, file alignment, stackreserve, cornflags).
* Custom Attributes not fully disassembled.
* .Net Types are being used instead of IL types.