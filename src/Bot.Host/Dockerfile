FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bot.Host/Bot.Host.csproj", "Bot.Host/"]
RUN dotnet restore "Bot.Host/Bot.Host.csproj"
COPY . .
WORKDIR "/src/Bot.Host"
RUN dotnet build "Bot.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.Host.dll"]
