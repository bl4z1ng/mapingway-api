FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
EXPOSE 80
EXPOSE 443
WORKDIR /src

COPY src/Mapingway.Common/*.csproj Mapingway.Common/
COPY src/Mapingway.Domain/*.csproj Mapingway.Domain/
COPY src/Mapingway.Application/*.csproj Mapingway.Application/
COPY src/Mapingway.Infrastructure/*.csproj Mapingway.Infrastructure/
COPY src/Mapingway.API/*.csproj Mapingway.API/

#RUN dotnet restore Mapingway.API/Mapingway.API.csproj

COPY src/Mapingway.Common/ Mapingway.Common/
COPY src/Mapingway.Domain/ Mapingway.Domain/
COPY src/Mapingway.Application/ Mapingway.Application/
COPY src/Mapingway.Infrastructure/ Mapingway.Infrastructure/
COPY src/Mapingway.API/ Mapingway.API/

WORKDIR Mapingway.API/

FROM build AS publish
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /src
COPY --from=publish /src/publish .

ENTRYPOINT [ "dotnet", "Mapingway.API.dll" ]