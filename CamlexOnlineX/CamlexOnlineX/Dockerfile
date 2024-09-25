FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY . /src

RUN dotnet restore "/src/CamlexOnlineX.csproj"
RUN dotnet publish "/src/CamlexOnlineX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV COMPlus_EnableDiagnostics=0

COPY --from=build /app/publish .

RUN addgroup --group camlex-online --gid 1001 \
&& adduser \
    --uid 1000 \
    --gid 1001 \
    "camlex-online"

RUN chown -R 1000:1001 /app
USER 1000:1001

ENTRYPOINT ["dotnet", "CamlexOnlineX.dll"]