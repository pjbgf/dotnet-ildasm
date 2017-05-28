#!/bin/bash


dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f netcoreapp1.0 $@
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f netstandard1.5 $@
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -c Release -f netstandard1.6 $@


if [[ ! -v RUN_MONO_TESTS ]]
then

    echo "Not building projects for mono."

else

    dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f net45 $@
    dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -c Release -f net45 $@

fi