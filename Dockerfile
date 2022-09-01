FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["km.Translate.Api/km.Translate.Api.csproj", "km.Translate.Api/"]
RUN dotnet restore "km.Translate.Api/km.Translate.Api.csproj"
COPY . .
WORKDIR "/src/km.Translate.Api"
RUN dotnet build "km.Translate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "km.Translate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "km.Translate.Api.dll"]
