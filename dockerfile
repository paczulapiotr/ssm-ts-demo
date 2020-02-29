FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy project files
COPY *.sln .
COPY GRPC.API/ GRPC.API/

# restore project dependencies
RUN dotnet restore
WORKDIR /app/GRPC.API
RUN dotnet publish -c Release -o ../prod

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/prod .
ENV HOSTING_URL http://+:5000/
ENTRYPOINT [ "dotnet", "GRPC.API.dll" ]