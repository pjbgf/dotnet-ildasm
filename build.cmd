dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f netcoreapp1.0 %*
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f netstandard1.5 %*
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -c Release -f net45 %*

dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -c Release -f net45 %*
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -c Release -f netstandard1.6 %*