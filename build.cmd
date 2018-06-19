dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.1 %*

dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f net45 %*
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 %*