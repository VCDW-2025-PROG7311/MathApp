FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
RUN mkdir /keys && chown app:app /keys
RUN apt-get update && apt-get install -y libgssapi-krb5-2

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG FirebaseMathApp
ARG Math_DB

ENV FirebaseMathApp=${FirebaseMathApp}
ENV Math_DB=${Math_DB}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .

RUN dotnet restore "./MathApp.csproj"

RUN dotnet build "./MathApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MathApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "MathApp.dll"]