FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj de todas as camadas
COPY ["BookStore.Api/BookStore.Api.csproj", "BookStore.Api/"]
COPY ["BookStore.Application/BookStore.Application.csproj", "BookStore.Application/"]
COPY ["BookStore.Domain/BookStore.Domain.csproj", "BookStore.Domain/"]
COPY ["BookStore.Infrastructure/BookStore.Infrastructure.csproj", "BookStore.Infrastructure/"]
COPY ["BookStore.SharedKernel/BookStore.SharedKernel.csproj", "BookStore.SharedKernel/"]

# Restaura as dependências
RUN dotnet restore "BookStore.Api/BookStore.Api.csproj"

# Copia todos os arquivos restantes da estrutura
COPY . .

# Compila a aplicação
WORKDIR "/src/BookStore.Api"
RUN dotnet build "BookStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookStore.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStore.Api.dll"]
