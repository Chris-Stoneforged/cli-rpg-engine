publish-game:
	dotnet publish src/Game -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o builds/Game
