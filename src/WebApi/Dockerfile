FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Estore/src/Application/Application.csproj", "src/Application/"]
COPY ["Estore/src/Domain/Domain.csproj", "src/Domain/"]
COPY ["Estore/src/Infrastructure/Persistence/Persistence.csproj", "src/Infrastructure/Persistence/"]

COPY ["Estore/src/Infrastructure/Services/Services.csproj", "src/Infrastructure/Services/"]
COPY ["Estore/src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/" ]

RUN dotnet restore "Estore/src/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build  -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
