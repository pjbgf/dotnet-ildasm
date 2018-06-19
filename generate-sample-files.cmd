@ECHO OFF

dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f net45 -c Release -o ../dotnet-ildasm.Benchmarks/Sample
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 -c Release -o ../dotnet-ildasm.Benchmarks/Sample