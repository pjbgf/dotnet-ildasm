dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp1.0 %*
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.0 %*
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netstandard1.5 %*
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f net45 %*

dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f net45 %*
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 %*