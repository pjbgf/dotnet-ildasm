#!/bin/bash

dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netstandard1.5 $@
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 $@


if [[ ! -v BUILD_NETCORE_10 ]]
then

    dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp1.0 $@

fi

if [[ ! -v BUILD_NETCORE_20 ]]
then

    dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.0 $@

fi

if [[ ! -v BUILD_NETCORE_21 ]]
then

    dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.1 $@

fi


if [[ ! -v RUN_MONO_TESTS ]]
then

    echo "Not building projects for mono."

else

    dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f net45 $@
    dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f net45 $@

fi