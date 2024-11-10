FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RR.DiceDuel/RR.DiceDuel.csproj", "RR.DiceDuel/"]
RUN dotnet restore "RR.DiceDuel/RR.DiceDuel.csproj"
COPY . .
WORKDIR "/src/RR.DiceDuel"
RUN dotnet build "RR.DiceDuel.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RR.DiceDuel.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT="Production"
ENTRYPOINT ["dotnet", "RR.DiceDuel.dll"]