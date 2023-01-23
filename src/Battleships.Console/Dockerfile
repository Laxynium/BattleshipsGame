FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Battleships.Console/Battleships.Console.csproj", "src/Battleships.Console/"]
RUN dotnet restore "src/Battleships.Console/Battleships.Console.csproj"
COPY . .
WORKDIR "/src/src/Battleships.Console"
RUN dotnet build "Battleships.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleships.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleships.Console.dll"]
