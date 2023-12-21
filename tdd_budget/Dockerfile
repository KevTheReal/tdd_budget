FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["tdd_budget/tdd_budget.csproj", "tdd_budget/"]
RUN dotnet restore "tdd_budget/tdd_budget.csproj"
COPY . .
WORKDIR "/src/tdd_budget"
RUN dotnet build "tdd_budget.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tdd_budget.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tdd_budget.dll"]
