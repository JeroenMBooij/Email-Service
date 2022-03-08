#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EmailService.Web.csproj", "EmailService.Web/"]
RUN dotnet restore "EmailService.Web/EmailService.Web.csproj"
COPY ["Dockerfile", "../"]
COPY . .
WORKDIR "/src/EmailService.Web"
RUN dotnet build "EmailService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailService.Web.dll"]