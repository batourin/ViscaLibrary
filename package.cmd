nuget pack ViscaLibrary\ViscaLibrary.nuspec -OutputDirectory ViscaLibrary\bin\Release
nuget push ViscaLibrary\bin\Release\ViscaLibrary.1.0.99.nupkg  -Source https://api.nuget.org/v3/index.json
