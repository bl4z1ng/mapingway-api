FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_HTTPS_PORTS=443

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

#enabling globalization for alpine image
RUN apk add --no-cache \
icu-data-full \
icu-libs

FROM mcr.microsoft.com/dotnet/sdk:8.0 as publish
WORKDIR /src

COPY ["src/Mapingway.SharedKernel/Mapingway.SharedKernel.csproj", "Mapingway.SharedKernel/Mapingway.SharedKernel.csproj"]
COPY ["src/Mapingway.Domain/Mapingway.Domain.csproj", "Mapingway.Domain/Mapingway.Domain.csproj"]
COPY ["src/Mapingway.Application/Mapingway.Application.csproj", "Mapingway.Application/Mapingway.Application.csproj"]
COPY ["src/Mapingway.Infrastructure/Mapingway.Infrastructure.csproj", "Mapingway.Infrastructure/Mapingway.Infrastructure.csproj"]
COPY ["src/Mapingway.Presentation/Mapingway.Presentation.csproj", "Mapingway.Presentation/Mapingway.Presentation.csproj"]
COPY ["src/Mapingway.API/Mapingway.API.csproj", "Mapingway.API/Mapingway.API.csproj"]

RUN dotnet restore "Mapingway.API/Mapingway.API.csproj"

COPY ["src/Mapingway.SharedKernel/", "Mapingway.SharedKernel/"]
COPY ["src/Mapingway.Domain/", "Mapingway.Domain/"]
COPY ["src/Mapingway.Application/", "Mapingway.Application/"]
COPY ["src/Mapingway.Infrastructure/", "Mapingway.Infrastructure/"]
COPY ["src/Mapingway.Presentation/", "Mapingway.Presentation/"]
COPY ["src/Mapingway.API/", "Mapingway.API/"]

WORKDIR "Mapingway.API/"
RUN dotnet build -c Release --no-restore
RUN dotnet publish -c Release --no-build -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Mapingway.API.dll" ]