dotnet restore
dotnet build
cd ArtAlley
dotnet publish -c Release -o .build/out
docker build -t chernikov/artalley .
docker push chernikov/artalley
