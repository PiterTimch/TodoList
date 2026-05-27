FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore as distinct layers
COPY Api/TodoListApi/TodoListApi.csproj Api/TodoListApi/
COPY Api/BLL/BLL.csproj Api/BLL/
COPY Api/DAL/DAL.csproj Api/DAL/
RUN dotnet restore Api/TodoListApi/TodoListApi.csproj

# Copy the rest of the source code
COPY Api/ Api/

WORKDIR /src/Api/TodoListApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TodoListApi.dll"]
