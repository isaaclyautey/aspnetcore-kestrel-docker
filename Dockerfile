FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY PersonalSite/*.csproj ./PersonalSite/
RUN dotnet restore

# copy everything else and build app
COPY PersonalSite/. ./PersonalSite/
WORKDIR /app/PersonalSite
EXPOSE 80/tcp
EXPOSE 443/tcp
EXPOSE 1337/tcp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/PersonalSite/out ./
ENTRYPOINT ["dotnet", "PersonalSite.dll"]
