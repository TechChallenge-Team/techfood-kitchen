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
COPY ["src/TechFood.Kitchen.Api/TechFood.Kitchen.Api.csproj", "TechFood.Kitchen.Api/"]
COPY ["src/TechFood.Kitchen.Application/TechFood.Kitchen.Application.csproj", "TechFood.Kitchen.Application/"]
COPY ["src/TechFood.Kitchen.Contracts/TechFood.Kitchen.Contracts.csproj", "TechFood.Kitchen.Contracts/"]
COPY ["src/TechFood.Kitchen.Domain/TechFood.Kitchen.Domain.csproj", "TechFood.Kitchen.Domain/"]
COPY ["src/TechFood.Kitchen.Infra/TechFood.Kitchen.Infra.csproj", "TechFood.Kitchen.Infra/"]
RUN dotnet nuget add source --username ${GITHUB_ACTOR} --password ${GITHUB_TOKEN} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/TechChallenge-Team/index.json"
RUN dotnet restore "./TechFood.Kitchen.Api/TechFood.Kitchen.Api.csproj"
COPY src/ .
WORKDIR /src/TechFood.Kitchen.Api
RUN dotnet build "./TechFood.Kitchen.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechFood.Kitchen.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechFood.Kitchen.Api.dll"]
