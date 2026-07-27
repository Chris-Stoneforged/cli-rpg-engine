publish-game:
	dotnet publish src/Game -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o builds/Game

rg:
	dotnet run --project src/Game

barg:
	dotnet build src/Game && dotnet run --project src/Game
