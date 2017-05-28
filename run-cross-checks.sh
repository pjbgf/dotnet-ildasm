#!/bin/bash


echo "Executing dotnet core ildasm against netstandard1.6 library..."
dotnet run --framework netcoreapp1.0 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il
   

if [[ ! -v RUN_MONO_TESTS ]]
then

    echo "Not running tests for mono."

else
   
    echo "Executing dotnet core ildasm against net45 library..."
    dotnet run --framework netcoreapp1.0 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/net45/dotnet-ildasm.Sample.dll -o netcore_net45.il
    
    echo "Executing net45 ildasm.exe against netstandard1.6 library..."
    mono src/dotnet-ildasm/bin/Release/net45/dotnet-ildasm.exe src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o net45_netcore.il
    
    echo "Executing net45 ildasm.exe against net45 library..."
    mono src/dotnet-ildasm/bin/Release/net45/dotnet-ildasm.exe src/dotnet-ildasm.Sample/bin/Release/net45/dotnet-ildasm.Sample.dll -o net45_net45.il
    
fi
