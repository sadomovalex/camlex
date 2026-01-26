FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY . /src

RUN dotnet restore "/src/CamlexOnlineX.csproj"
RUN dotnet publish "/src/CamlexOnlineX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update && apt-get install -y adduser

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV COMPlus_EnableDiagnostics=0

COPY --from=build /app/publish .

RUN addgroup --group camlex-online --gid 2001 \
&& adduser \
    --uid 2000 \
    --gid 2001 \
    "camlex-online"

RUN chown -R 2000:2001 /app
USER 2000:2001

ENTRYPOINT ["dotnet", "CamlexOnlineX.dll"]