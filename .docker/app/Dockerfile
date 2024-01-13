FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_HTTPS_PORTS=8081
WORKDIR /src

COPY src/Mapingway.SharedKernel/ Mapingway.SharedKernel/
COPY src/Mapingway.Domain/ Mapingway.Domain/
COPY src/Mapingway.Application/ Mapingway.Application/
COPY src/Mapingway.Infrastructure/ Mapingway.Infrastructure/
COPY src/Mapingway.Presentation/ Mapingway.Presentation/
COPY src/Mapingway.API/ Mapingway.API/

WORKDIR Mapingway.API/

FROM build AS publish
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /src
COPY --from=publish /src/publish .

ENTRYPOINT [ "dotnet", "Mapingway.API.dll" ]