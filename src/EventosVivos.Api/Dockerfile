#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventosVivos.Api/EventosVivos.Api.csproj", "EventosVivos.Api/"]
COPY ["EventosVivos.Application/EventosVivos.Application.csproj", "EventosVivos.Application/"]
COPY ["EventosVivos.Domain/EventosVivos.Domain.csproj", "EventosVivos.Domain/"]
COPY ["EventosVivos.Infrastructure/EventosVivos.Infrastructure.csproj", "EventosVivos.Infrastructure/"]
RUN dotnet restore "./EventosVivos.Api/EventosVivos.Api.csproj"
COPY . .
WORKDIR "/src/EventosVivos.Api"
RUN dotnet build "./EventosVivos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventosVivos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventosVivos.Api.dll"]