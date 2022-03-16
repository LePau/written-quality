# Use Microsoft's official build .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13 AS build
WORKDIR /app

# Copy local code to the container image.
COPY . ./

# Install production dependencies.
RUN dotnet restore

# Build a release artifact.
RUN dotnet publish -c Release -o out

# Use Microsoft's official runtime .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.13 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# https://github.com/grpc/grpc/issues/21446#issuecomment-572305508
RUN echo 'http://dl-cdn.alpinelinux.org/alpine/v3.8/main' >> /etc/apk/repositories && apk update --no-cache && apk add --no-cache bash libc6-compat=1.1.19-r11

# Run the web service on container startup.
ENTRYPOINT ["dotnet", "WrittenQuality.Api.dll", "--allow-http"]