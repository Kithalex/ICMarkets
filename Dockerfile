# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy everything into the container
COPY . .

# restore and publish the API project explicitly
RUN dotnet restore src/Api/Api.csproj
RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish


# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# copy published output from build stage
COPY --from=build /app/publish .

# port expose
EXPOSE 80

# run the API
ENTRYPOINT ["dotnet", "Api.dll"]
