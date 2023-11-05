FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TodoList.WebApi/TodoList.WebApi.csproj", "src/TodoList.WebApi/"]
RUN dotnet restore "src/TodoList.WebApi/TodoList.WebApi.csproj"
COPY . .
WORKDIR "/src/src/TodoList.WebApi"
RUN dotnet build "TodoList.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoList.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoList.WebApi.dll"]
