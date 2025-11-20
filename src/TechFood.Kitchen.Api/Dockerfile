ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

FROM ${DOTNET_RUNTIME} AS base
RUN apt-get update && \
    apt-get install wget -y
WORKDIR /app
EXPOSE 8080

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN
ARG GITHUB_ACTOR
WORKDIR /src
COPY ["src/TechFood.Order.Api/TechFood.Order.Api.csproj", "TechFood.Order.Api/"]
COPY ["src/TechFood.Order.Application/TechFood.Order.Application.csproj", "TechFood.Order.Application/"]
COPY ["src/TechFood.Order.Contracts/TechFood.Order.Contracts.csproj", "TechFood.Order.Contracts/"]
COPY ["src/TechFood.Order.Domain/TechFood.Order.Domain.csproj", "TechFood.Order.Domain/"]
COPY ["src/TechFood.Order.Infra/TechFood.Order.Infra.csproj", "TechFood.Order.Infra/"]
RUN dotnet nuget add source --username ${GITHUB_ACTOR} --password ${GITHUB_TOKEN} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/TechChallenge-Team/index.json"
RUN dotnet restore "./TechFood.Order.Api/TechFood.Order.Api.csproj"
COPY src/ .
WORKDIR /src/TechFood.Order.Api
RUN dotnet build "./TechFood.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechFood.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechFood.Order.Api.dll"]
