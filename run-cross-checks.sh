#!/bin/bash



if [[ ! -v BUILD_NETCORE_10 ]]
then

    echo "[dotnet core 1.0] Executing ildasm against netstandard1.6 library..."
    dotnet run --framework netcoreapp1.0 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il

fi

if [[ ! -v BUILD_NETCORE_20 ]]
then

    echo "[dotnet core 2.0] Executing ildasm against netstandard1.6 library..."
    dotnet run --framework netcoreapp2.0 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il

fi

if [[ ! -v BUILD_NETCORE_20 ]]
then

    echo "[dotnet core 2.1] Executing ildasm against netstandard1.6 library..."
    dotnet run --framework netcoreapp2.1 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il

fi


if [[ ! -v RUN_MONO_TESTS ]]
then

    echo "Not running tests for mono."

else
   
    echo "Executing dotnet core ildasm against net45 library..."
    dotnet run --framework netcoreapp1.0 --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/net45/dotnet-ildasm.Sample.exe -o netcore_net45.il
    
    echo "Executing net45 ildasm.exe against netstandard1.6 library..."
    mono src/dotnet-ildasm/bin/Release/net45/dotnet-ildasm.exe src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o net45_netcore.il
    
    echo "Executing net45 ildasm.exe against net45 library..."
    mono src/dotnet-ildasm/bin/Release/net45/dotnet-ildasm.exe src/dotnet-ildasm.Sample/bin/Release/net45/dotnet-ildasm.Sample.exe -o net45_net45.il
    
fi

if [[ ! -v RUN_REASSEMBLE_TESTS ]]
then

    echo "Not running reassembling tests."

else

	echo "Reassembling .IL's onto Portable executable files again."
	ilasm netcore_netstandard16.il /dll /output:netcore_netstandard16.dll
	ilasm netcore_net45.il /exe /output:netcore_net45.exe
	ilasm net45_netcore.il /exe /output:net45_netcore.exe
	ilasm net45_net45.il /exe /output:net45_net45.exe

fi