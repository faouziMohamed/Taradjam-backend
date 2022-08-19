FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataTranslateApi/DataTranslateApi.csproj", "DataTranslateApi/"]
RUN dotnet restore "DataTranslateApi/DataTranslateApi.csproj"
COPY . .
WORKDIR "/src/DataTranslateApi"
RUN dotnet build "DataTranslateApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataTranslateApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataTranslateApi.dll"]
