#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1809 AS build
WORKDIR /src
COPY ["TestingApp.API/TestingApp.API.csproj", "TestingApp.API/"]
RUN dotnet restore "TestingApp.API/TestingApp.API.csproj"
COPY . .
WORKDIR "/src/TestingApp.API"
RUN dotnet build "TestingApp.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestingApp.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestingApp.API.dll"]
