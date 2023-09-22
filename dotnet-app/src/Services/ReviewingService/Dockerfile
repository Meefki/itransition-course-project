FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/Reviewing.API/Reviewing.API.csproj", "/Reviewing.API/"]
COPY ["/Reviewing.Infrastructure/Reviewing.Infrastructure.csproj", "/Reviewing.Infrastructure/"]
COPY ["/Reviewing.Application/Reviewing.Application.csproj", "/Reviewing.Application/"]
COPY ["/Reviewing.Domain/Reviewing.Domain.csproj", "/Reviewing.Domain/"]
RUN dotnet restore "/Reviewing.API/Reviewing.API.csproj"
COPY . .
WORKDIR "/src/Reviewing.API"
RUN dotnet build "Reviewing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reviewing.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reviewing.API.dll"]