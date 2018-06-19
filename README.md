# Dot Net IL Disassembler  
[![Windows Build](https://ci.appveyor.com/api/projects/status/is3qcb0gnf018vx6/branch/master?svg=true)](https://ci.appveyor.com/project/pjbgf/dotnet-ildasm/branch/master) 
[![CircleCI](https://circleci.com/gh/pjbgf/dotnet-ildasm.svg?style=svg)](https://circleci.com/gh/pjbgf/dotnet-ildasm)
[![Nuget](https://img.shields.io/nuget/dt/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Nuget](https://img.shields.io/nuget/v/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  

# How to Install?
The project was created as a global CLI tool, therefore you can install with a single commad:  

`dotnet tool install -g dotnet-ildasm`


# Syntax
```
dotnet ildasm <ASSEMBLY_PATH> <-t|--text>
dotnet ildasm <ASSEMBLY_PATH> <-o|--output>
dotnet ildasm <ASSEMBLY_PATH> <-i|--item>
dotnet ildasm <-h|--help>
```

# Description
The `dotnet ildasm` provides a command-line IL dissassembler. Simply send the assembly path as a parameter and as a result a file of the same name will be created.

# Options
`-t`  
Output results to the command line.

`-i`  
Filter results by method and/or classes to be disassembled.

`-o`  
Define the file to be created with the IL.

# Examples
Output results to the command line:
```
dotnet ildasm myassembly.dll -t
```

Filter results by method and/or classes to be disassembled:
```
dotnet ildasm myassembly.dll -t -i ClassName
dotnet ildasm myassembly.dll -t -i ::MethodName
dotnet ildasm myassembly.dll -t -i ClassName::MethodName
```

Define the file to be created with the output: 
```
dotnet ildasm myassembly.dll -o disassembledAssembly.il
```
  
# Supported .Net Versions
The CLI application current supports:

* .Net Core App 1.0, 1.1, 2.0, 2.1
* .Net Standard 1.5
* .Net Framework 4.5

# Powered by
This tool was developed and is maintained with JetBrains Rider: the cross-platform and lightweight .NET/C# IDE which comes with ReSharper integrated. For more information check [JetBrains' website](https://www.jetbrains.com/rider).
