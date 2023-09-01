FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/ReviewingService/Reviewing.API/Reviewing.API.csproj", "src/Services/ReviewingService/Reviewing.API/"]
COPY ["src/Services/ReviewingService/Reviewing.Infrastructure/Reviewing.Infrastructure.csproj", "src/Services/ReviewingService/Reviewing.Infrastructure/"]
COPY ["src/Services/ReviewingService/Reviewing.Application/Reviewing.Application.csproj", "src/Services/ReviewingService/Reviewing.Application/"]
COPY ["src/Services/ReviewingService/Reviewing.Domain/Reviewing.Domain.csproj", "src/Services/ReviewingService/Reviewing.Domain/"]
RUN dotnet restore "src/Services/ReviewingService/Reviewing.API/Reviewing.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ReviewingService/Reviewing.API"
RUN dotnet build "Reviewing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reviewing.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reviewing.API.dll"]