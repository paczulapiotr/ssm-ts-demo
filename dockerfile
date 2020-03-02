FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy project files
COPY *.sln .
COPY DemoAPI.Server/ DemoAPI.Server/
COPY DemoAPI.Common/ DemoAPI.Common/
COPY DemoAPI.Client/ DemoAPI.Client/

# restore project dependencies
RUN dotnet restore
WORKDIR /app/DemoAPI.Server
RUN dotnet publish -c Release -o ../prod

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/prod .

ENTRYPOINT [ "dotnet", "DemoAPI.Server.dll" ]