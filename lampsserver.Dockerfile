FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

RUN dotnet restore "./LAMPSServer.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "LAMPSServer.dll"]




# docker build -t lampsserver .
# docker run -d -p 8080:80 --name lampsserver lampsserver
# docker run --rm -it lampsserver:latest

# docker exec -t -i lampsserver /bin/bash