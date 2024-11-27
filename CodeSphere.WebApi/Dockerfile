
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development 
EXPOSE 8080		
EXPOSE 8081	

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["CodeSphere.WebApi/CodeSphere.WebApi.csproj", "CodeSphere.WebApi/"]
COPY ["CodeSphere.Application/CodeSphere.Application.csproj", "CodeSphere.Application/"]
COPY ["CodeSphere.Domain/CodeSphere.Domain.csproj", "CodeSphere.Domain/"]
COPY ["CodeSphere.Infrastructure/CodeSphere.Infrastructure.csproj", "CodeSphere.Infrastructure/"]
RUN dotnet restore "./CodeSphere.WebApi/CodeSphere.WebApi.csproj"
COPY . .
WORKDIR "/src/CodeSphere.WebApi"
RUN dotnet build "./CodeSphere.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CodeSphere.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeSphere.WebApi.dll"]