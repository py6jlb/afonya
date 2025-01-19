#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

RUN apt-get update && apt-get install -y libfontconfig1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Bot/Afonya.Bot.WebWorker/Afonya.Bot.WebWorker.csproj", "Services/Bot/Afonya.Bot.WebWorker/"]
COPY ["Services/Bot/Afonya.Bot.Logic/Afonya.Bot.Logic.csproj", "Services/Bot/Afonya.Bot.Logic/"]
COPY ["Services/Bot/Afonya.Bot.Interfaces/Afonya.Bot.Interfaces.csproj", "Services/Bot/Afonya.Bot.Interfaces/"]
COPY ["Services/Bot/Afonya.Bot.Domain/Afonya.Bot.Domain.csproj", "Services/Bot/Afonya.Bot.Domain/"]
COPY ["Services/Bot/Afonya.Bot.Infrastructure/Afonya.Bot.Infrastructure.csproj", "Services/Bot/Afonya.Bot.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "Services/Bot/Afonya.Bot.WebWorker/Afonya.Bot.WebWorker.csproj"
COPY . .
WORKDIR "/src/Services/Bot/Afonya.Bot.WebWorker"
RUN dotnet build "Afonya.Bot.WebWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Afonya.Bot.WebWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afonya.Bot.WebWorker.dll"]