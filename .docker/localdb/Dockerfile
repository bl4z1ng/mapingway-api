FROM mcr.microsoft.com/dotnet/sdk:7.0 AS migration
WORKDIR /src

COPY src/Mapingway.Common/*.csproj Mapingway.Common/
COPY src/Mapingway.Domain/*.csproj Mapingway.Domain/
COPY src/Mapingway.Application/*.csproj Mapingway.Application/
COPY src/Mapingway.Infrastructure/*.csproj Mapingway.Infrastructure/
COPY src/Mapingway.API/*.csproj Mapingway.API/

COPY src/Mapingway.Common/ Mapingway.Common/
COPY src/Mapingway.Domain/ Mapingway.Domain/
COPY src/Mapingway.Application/ Mapingway.Application/
COPY src/Mapingway.Infrastructure/ Mapingway.Infrastructure/
COPY src/Mapingway.API/ Mapingway.API/

ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef --version 7.*
ENV PATH $PATH:/

WORKDIR migration/
WORKDIR ../

RUN dotnet ef migrations script  \
    --project Mapingway.Infrastructure/Mapingway.Infrastructure.csproj  \
    --startup-project Mapingway.API/Mapingway.API.csproj  \
    --context Mapingway.Infrastructure.Persistence.ApplicationDbContext  \
    --configuration Production  \
    --idempotent  \
    --output migration/init.sql 

FROM postgres AS final

COPY --from=migration src/migration/ docker-entrypoint-initdb.d/

EXPOSE 5432