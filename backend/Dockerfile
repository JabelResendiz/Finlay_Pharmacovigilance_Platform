FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Finlay.PharmaVigilance.sln", "./"]
COPY ["Finlay.PharmaVigilance.API/Finlay.PharmaVigilance.API.csproj", "Finlay.PharmaVigilance.API/"]
COPY ["Finlay.PharmaVigilance.Application/Finlay.PharmaVigilance.Application.csproj", "Finlay.PharmaVigilance.Application/"]
COPY ["Finlay.PharmaVigilance.Domain/Finlay.PharmaVigilance.Domain.csproj", "Finlay.PharmaVigilance.Domain/"]
COPY ["Finlay.PharmaVigilance.Infrastructure/Finlay.PharmaVigilance.Infrastructure.csproj", "Finlay.PharmaVigilance.Infrastructure/"]

RUN dotnet restore "Finlay.PharmaVigilance.sln"

COPY . .
RUN dotnet publish "Finlay.PharmaVigilance.API/Finlay.PharmaVigilance.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5137
EXPOSE 5137
ENTRYPOINT ["dotnet", "Finlay.PharmaVigilance.API.dll"]
