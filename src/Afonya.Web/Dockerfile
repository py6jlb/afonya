FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Afonya.Web/Afonya.Web.csproj", "src/Afonya.Web/"]
RUN dotnet restore "src/Afonya.Web/Afonya.Web.csproj"
COPY . .
WORKDIR "/src/src/Afonya.Web"
RUN dotnet build "Afonya.Web.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Afonya.Web.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afonya.Web.dll"]
