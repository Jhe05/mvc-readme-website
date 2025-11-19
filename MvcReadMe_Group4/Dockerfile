FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ./MvcReadMe_Group4/*.csproj ./MvcReadMe_Group4/
RUN dotnet restore ./MvcReadMe_Group4/MvcReadMe_Group4.csproj

# Copy everything and build
COPY . .
WORKDIR /src/MvcReadMe_Group4
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Entrypoint will respect $PORT if set by hosting platform
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish ./

ENTRYPOINT ["/bin/bash","-c","export ASPNETCORE_URLS=http://+:${PORT:-8080} && dotnet MvcReadMe_Group4.dll"]
