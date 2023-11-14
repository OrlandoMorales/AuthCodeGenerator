FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ArivalBank.2fa.WebApi/ArivalBank.2fa.WebApi.csproj", "ArivalBank.2fa.WebApi/"]
RUN dotnet restore "ArivalBank.2fa.WebApi/ArivalBank.2fa.WebApi.csproj"
COPY . .
WORKDIR "/src/ArivalBank.2fa.WebApi"
RUN dotnet build "ArivalBank.2fa.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArivalBank.2fa.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArivalBank.2fa.WebApi.dll"]