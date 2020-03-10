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
ENV certPassword 1234
ENV appPort 5000
EXPOSE ${appPort}
WORKDIR /app
COPY --from=build /app/prod .

# Configure SSL cert
ENV certPassword 1234
ENV useCert yes
RUN openssl genrsa -des3 -passout pass:${certPassword} -out server.key 2048
RUN openssl rsa -passin pass:${certPassword} -in server.key -out server.key
RUN openssl req -sha256 -new -key server.key -out server.csr -subj '/CN=localhost'
RUN openssl x509 -req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt
RUN openssl pkcs12 -export -out cert.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:${certPassword}

ENTRYPOINT [ "dotnet", "DemoAPI.Server.dll" ]
