#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.6-bullseye-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PowerCost.API/PowerCost.API.csproj", "PowerCost.API/"]
RUN dotnet restore "PowerCost.API/PowerCost.API.csproj"
COPY . .
WORKDIR "/src/PowerCost.API"
RUN dotnet build "PowerCost.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerCost.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PowerCost.API.dll"]