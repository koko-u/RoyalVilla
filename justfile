_default:
  @just --list
  
run:
  dotnet run --project ./Api/RoyalVilla.Api.csproj --launch-profile https

watch:
  dotnet watch run --quiet --project ./Api/RoyalVilla.Api.csproj --launch-profile https

