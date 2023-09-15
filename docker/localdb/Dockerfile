FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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

WORKDIR Mapingway.API/
RUN dotnet build -c Release -o ../build

RUN ls
RUN echo $(pwd)

ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef --version 7.*
ENV PATH $PATH:/

WORKDIR ..
RUN ls
RUN echo $(pwd)
RUN dotnet ef migrations bundle  \
    --project Mapingway.Infrastructure/Mapingway.Infrastructure.csproj  \
    --startup-project Mapingway.API/Mapingway.API.csproj  \
    --context Mapingway.Infrastructure.Persistence.ApplicationDbContext  \
    --self-contained  \
    --configuration Production  \
    --no-build  \
    --force  \
    --verbose  \
    --runtime linux-x64  \
    --output build/efbundle.dll 

RUN ls
RUN echo $(pwd)

FROM postgres AS bundle

ENV POSTGRES_DB Mapingway
ENV POSTGRES_USER postgres
ENV POSTGRES_PASSWORD postgres
EXPOSE 5432

WORKDIR /src
COPY --from=build src/build/ .

RUN efbundle.dll --connection "User ID=postgres;Server=localhost;Host=localhost;Port=5432;Database=Mapingway;Username=postgres;Password=postgres;Integrated Security=true;Pooling=true;Encoding=UTF8",