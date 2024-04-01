build:
	dotnet build

pack:
	dotnet pack

run: 
	dotnet run --project .\src

format:
	dotnet format

publish: build
	dotnet publish -c Release -o ./publish

install: pack
	dotnet tool install -g --add-source ./nupkg Cli

update: pack
	dotnet tool update -g --add-source ./nupkg Cli

uninstall:
	dotnet tool uninstall -g Cli
