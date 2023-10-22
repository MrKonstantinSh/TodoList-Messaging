FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TodoList.EmailSender/TodoList.EmailSender.csproj", "src/TodoList.EmailSender/"]
RUN dotnet restore "src/TodoList.EmailSender/TodoList.EmailSender.csproj"
COPY . .
WORKDIR "/src/src/TodoList.EmailSender"
RUN dotnet build "TodoList.EmailSender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoList.EmailSender.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoList.EmailSender.dll"]
