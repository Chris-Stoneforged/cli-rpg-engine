publish-game:
	dotnet publish src/Game -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o builds/Game

rg:
	dotnet run "./campaigns/test/campaign.db" --project src/Game

barg:
	dotnet build src/Game && dotnet run "./campaigns/test/campaign.db" --project src/Game

bc:
	dotnet build src/Editor && dotnet run --project src/Editor -- build-campaign -p "./campaigns/test" -o "./campaigns/test/campaign.db"
