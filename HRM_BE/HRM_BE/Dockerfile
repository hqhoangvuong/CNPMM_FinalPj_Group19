#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HRM_BE/HRM.API.csproj", "HRM_BE/"]
COPY ["HRM.Core/HRM.Core.csproj", "HRM.Core/"]
COPY ["HRM.Service/HRM.Service.csproj", "HRM.Service/"]
RUN dotnet restore "HRM_BE/HRM.API.csproj"
COPY . .
WORKDIR "/src/HRM_BE"
RUN dotnet build "HRM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HRM.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRM.API.dll"]