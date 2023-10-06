FROM mcr.microsoft.com/dotnet/sdk:7.0 AS migration
WORKDIR /src

ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef --version 7.0.11
ENV PATH $PATH:/

RUN mkdir migration/

COPY src/Mapingway.Common/ Mapingway.Common/
COPY src/Mapingway.Domain/ Mapingway.Domain/
COPY src/Mapingway.Application/ Mapingway.Application/
COPY src/Mapingway.Infrastructure/ Mapingway.Infrastructure/
COPY src/Mapingway.API/ Mapingway.API/

RUN dotnet ef migrations script  \
    --project Mapingway.Infrastructure/Mapingway.Infrastructure.csproj  \
    --startup-project Mapingway.API/Mapingway.API.csproj  \
    --context Mapingway.Infrastructure.Persistence.ApplicationDbContext  \
    --configuration Production  \
    --idempotent  \
    --output migration/init.sql 

FROM postgres:16-alpine3.18 AS final

COPY --from=migration src/migration/ docker-entrypoint-initdb.d/

EXPOSE 5432