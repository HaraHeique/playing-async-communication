#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PAC.Producao/PAC.Producao.csproj", "src/PAC.Producao/"]
RUN dotnet restore "src/PAC.Producao/PAC.Producao.csproj"
COPY . .
WORKDIR "/src/src/PAC.Producao"
RUN dotnet build "PAC.Producao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PAC.Producao.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PAC.Producao.dll"]