
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ./HotelApp.Api/HotelApp.Api.csproj ./
COPY ./HotelApp.Core/HotelApp.Core.csproj ./
COPY ./HotelApp.Data/HotelApp.Data.csproj ./
COPY ./HotelApp.Jobs/HotelApp.Jobs.csproj ./

RUN dotnet restore HotelApp.Api.csproj
COPY . ./
RUN dotnet publish ./HotelApp.Api/HotelApp.Api.csproj  -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
#HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 CMD [ "curl -f http://localhost/health/liveness || exit 1" ]
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "HotelApp.Api.dll"]