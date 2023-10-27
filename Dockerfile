# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/

# Copie os arquivos .csproj para o contexto de construção
COPY ["Challenge.Balta.IBGE/Challenge.Balta.IBGE.csproj", "Challenge.Balta.IBGE/"]
COPY ["Challenge.Balta.IBGE.Infra/Challenge.Balta.IBGE.Infra.csproj", "Challenge.Balta.IBGE.Infra/"]
COPY ["Challenge.Balta.IBGE.Service/Challenge.Balta.IBGE.Service.csproj", "Challenge.Balta.IBGE.Service/"]
COPY ["Challenge.Balta.IBGE.Tests/Challenge.Balta.IBGE.Tests.csproj", "Challenge.Balta.IBGE.Tests/"]
COPY ["Challenge.Balta.IBGE.Domain/Challenge.Balta.IBGE.Domain.csproj", "Challenge.Balta.IBGE.Domain/"]

# Restaure as dependências para um projeto específico
RUN dotnet restore "Challenge.Balta.IBGE/Challenge.Balta.IBGE.csproj"

# Copie o restante do código da aplicação
COPY . .

# Build da aplicação para um projeto específico
RUN dotnet build "Challenge.Balta.IBGE/Challenge.Balta.IBGE.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "Challenge.Balta.IBGE/Challenge.Balta.IBGE.csproj" -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Challenge.Balta.IBGE.dll"]
