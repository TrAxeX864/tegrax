
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

RUN apt-get update

RUN mkdir /usr/src/app
WORKDIR /usr/src/app

COPY . .

WORKDIR /usr/src/app/TgVozderzhansBot
RUN dotnet new tool-manifest
RUN dotnet tool install dotnet-ef --version 5.0.1
RUN export PATH="$PATH:$HOME/.dotnet/tools/"

RUN dotnet ef database update

RUN mkdir /app

RUN dotnet publish -c release -o /app

RUN cp -R /usr/src/app/ProductionFiles/* /app
RUN cp /usr/src/app/.env /app/.env

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "TgVozderzhansBot.dll"]
