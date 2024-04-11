FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY . /app

EXPOSE 80

ENTRYPOINT ["dotnet", "DirectumCareerNightBot.dll"]