FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 61105

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY "MoneyMaker.sln" "MoneyMaker.sln"

COPY "Core/Api/Api.csproj" "Core/Api/Api.csproj"
COPY "Core/Domain/Domain.csproj" "Core/Domain/Domain.csproj"
COPY "Core/Simulator/StrategySimulator.csproj" "Core/Simulator/StrategySimulator.csproj"
COPY "Core/FunctionalTests/FunctionalTests.csproj" "Core/FunctionalTests/FunctionalTests.csproj"
COPY "Core/BackgroundTasks/BackgroundTasks.csproj" "Core/BackgroundTasks/BackgroundTasks.csproj"
COPY "Core/Infrastructure/Infrastructure.csproj" "Core/Infrastructure/Infrastructure.csproj"
COPY "Core/UnitTests/DomainTests.csproj" "Core/UnitTests/DomainTests.csproj"
COPY "Core/NeuralNetwork/NeuralNetwork.csproj" "Core/NeuralNetwork/NeuralNetwork.csproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "MoneyMaker.sln"

COPY . .
WORKDIR /src/Core/BackgroundTasks
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackgroundTasks.dll"]