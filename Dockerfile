FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WebApi/WebApi.csproj", "WebApi/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Identity/Identity.csproj", "Infrastructure/Identity/"]
COPY ["src/Infrastructure/Persistence.EntityFramework/Persistence.EntityFramework.csproj", "Infrastructure/Persistence.EntityFramework/"]
COPY ["src/Infrastructure/Services/Services.csproj", "Infrastructure/Services/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "SharedKernel/" ]


RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/"


RUN dotnet build  -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]

