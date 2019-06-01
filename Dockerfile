FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["efcore1.csproj", "efcore1/"]
RUN dotnet restore "efcore1/efcore1.csproj"
WORKDIR "/src/efcore1"
COPY . .

RUN dotnet build "efcore1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "efcore1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "efcore1.dll"]