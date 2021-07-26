# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

RUN apt-get update
RUN apt-get install -y nano

RUN mkdir /usr/src/app
WORKDIR /usr/src/app

COPY . .

RUN mkdir /app

RUN dotnet publish -c release -o /app

RUN cp -R /usr/src/app/ProductionFiles/* /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "TgVozderzhansBot.dll"]