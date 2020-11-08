FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

EXPOSE 80
ENV ASPNETCORE_URLS http://+:80

COPY .build/out .
ENTRYPOINT ["dotnet", "ArtAlley.dll"]