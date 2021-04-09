nuget pack ViscaLibrary\ViscaLibrary.nuspec -OutputDirectory ViscaLibrary\bin\Release
nuget push ViscaLibrary\bin\Release\ViscaLibrary.1.1.0.nupkg  -Source https://api.nuget.org/v3/index.json
nuget push ViscaLibrary\bin\Release\ViscaLibrary.1.1.0.nupkg  -Source "github"