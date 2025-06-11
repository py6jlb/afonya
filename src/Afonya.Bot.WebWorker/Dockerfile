FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Services/Bot/Afonya.Bot.WebWorker/Afonya.Bot.WebWorker.csproj", "src/Services/Bot/Afonya.Bot.WebWorker/"]
RUN dotnet restore "src/Services/Bot/Afonya.Bot.WebWorker/Afonya.Bot.WebWorker.csproj"
COPY . .
WORKDIR "/src/src/Services/Bot/Afonya.Bot.WebWorker"
RUN dotnet build "Afonya.Bot.WebWorker.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Afonya.Bot.WebWorker.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afonya.Bot.WebWorker.dll"]
