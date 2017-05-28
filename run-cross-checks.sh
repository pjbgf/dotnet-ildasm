#!/bin/bash

if [ ! type "$mono" > /dev/null; ] then

    ECHO Executing dotnet core ildasm against netstandard1.6 library...
    ECHO dotnet run --framework netcoreapp1.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj src\dotnet-ildasm.Sample\bin\Debug\netstandard1.6\dotnet-ildasm.Sample.dll -o netcore_netstandard16.il
    
    ECHO if %ERRORLEVEL% EQU 0 ECHO DONE
    
    
    ECHO Executing dotnet core ildasm against net45 library...
    ECHO dotnet run --framework netcoreapp1.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj src\dotnet-ildasm.Sample\bin\Debug\net45\dotnet-ildasm.Sample.dll -o netcore_net45.il
    
    if %ERRORLEVEL% EQU 0 ECHO DONE

else

    ECHO Executing net45 ildasm.exe against netstandard1.6 library...
    ECHO mono src\dotnet-ildasm\bin\debug\net45\dotnet-ildasm.exe src\dotnet-ildasm.Sample\bin\Debug\netstandard1.6\dotnet-ildasm.Sample.dll -o net45_netcore.il
    ECHO if %ERRORLEVEL% EQU 0 ECHO DONE
    
    ECHO Executing net45 ildasm.exe against net45 library...
    ECHO src\dotnet-ildasm\bin\debug\net45\dotnet-ildasm.exe src\dotnet-ildasm.Sample\bin\Debug\net45\dotnet-ildasm.Sample.dll -o net45_net45.il
    
    ECHO if %ERRORLEVEL% EQU 0 ECHO DONE
  
fi

