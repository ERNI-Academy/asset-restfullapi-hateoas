#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/ERNI.Api.Hateoas.Sample/ERNI.Api.Hateoas.Sample.csproj", "sample/ERNI.Api.Hateoas.Sample/"]
RUN dotnet restore "sample/ERNI.Api.Hateoas.Sample/ERNI.Api.Hateoas.Sample.csproj"
COPY . .
WORKDIR "/src/sample/ERNI.Api.Hateoas.Sample"
RUN dotnet build "ERNI.Api.Hateoas.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERNI.Api.Hateoas.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERNI.Api.Hateoas.Sample.dll"]