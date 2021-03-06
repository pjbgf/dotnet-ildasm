# Dot Net IL Disassembler  
[![CircleCI](https://circleci.com/gh/pjbgf/dotnet-ildasm.svg?style=shield)](https://circleci.com/gh/pjbgf/dotnet-ildasm)
[![Nuget](https://img.shields.io/nuget/dt/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Nuget](https://img.shields.io/nuget/v/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  

# Description
The `dotnet ildasm` provides a command-line IL dissassembler. Simply send the assembly path as a parameter and as a result you will get the IL contents of that assembly.

# Setup
The project was created as a global CLI tool, therefore you can install with a single command:  

`dotnet tool install -g dotnet-ildasm`

Note that for the command above to work, you need .NET Core SDK 2.1.300 or above installed in your machine.

# Syntax
```
dotnet ildasm <ASSEMBLY_PATH>
dotnet ildasm <ASSEMBLY_PATH> <-o|--output>
dotnet ildasm <ASSEMBLY_PATH> <-i|--item>
dotnet ildasm <-h|--help>
```

# Options
`-i`  
Filter results by method and/or classes to be disassembled.

`-o`  
Define the output file to be created with the assembly's IL.

# Examples
Output IL to the command line:
```
dotnet ildasm myassembly.dll
```

Filter results by method and/or classes to be disassembled, showing the result in the command line:
```
dotnet ildasm myassembly.dll -i ClassName
dotnet ildasm myassembly.dll -i ::MethodName
dotnet ildasm myassembly.dll -i ClassName::MethodName
```

Define the file to be created with the output: 
```
dotnet ildasm myassembly.dll -o disassembledAssembly.il
```
