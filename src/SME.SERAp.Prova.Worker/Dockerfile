#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["src/SME.SERAp.Prova.Worker/SME.SERAp.Prova.Worker.csproj", "src/SME.SERAp.Prova.Worker/"]
COPY ["src/SME.SERAp.Prova.IoC/SME.SERAp.Prova.IoC.csproj", "src/SME.SERAp.Prova.IoC/"]
COPY ["src/SME.SERAp.Prova.Dados/SME.SERAp.Prova.Dados.csproj", "src/SME.SERAp.Prova.Dados/"]
COPY ["src/SME.SERAp.Prova.Infra/SME.SERAp.Prova.Infra.csproj", "src/SME.SERAp.Prova.Infra/"]
COPY ["src/SME.SERAp.Prova.Dominio/SME.SERAp.Prova.Dominio.csproj", "src/SME.SERAp.Prova.Dominio/"]
COPY ["src/SME.SERAp.Prova.Aplicacao/SME.SERAp.Prova.Aplicacao.csproj", "src/SME.SERAp.Prova.Aplicacao/"]

RUN dotnet restore "src/SME.SERAp.Prova.Worker/SME.SERAp.Prova.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SERAp.Prova.Worker"
RUN dotnet build "SME.SERAp.Prova.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SERAp.Prova.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo

RUN apt-get update \
    && apt-get install -yq tzdata locales -y \
    && dpkg-reconfigure --frontend noninteractive tzdata \ 
    && locale-gen en_US.UTF-8 \
    && apt-get install -y --allow-unauthenticated \
    libc6-dev \
    libgdiplus \
    libx11-dev \
    && rm -rf /var/lib/apt/lists/*

# Configuração OpenSSL para suportar TLS 1.0/1.1 (necessário para SQL Server 2014 em Windows Server 2012 R2)
RUN echo "\n[openssl_init]" >> /etc/ssl/openssl.cnf
RUN sed -i '/[openssl_init]/a ssl_conf = ssl_configuration' /etc/ssl/openssl.cnf

RUN echo "\n[ssl_configuration]" >> /etc/ssl/openssl.cnf \
    && echo "system_default = tls_system_default" >> /etc/ssl/openssl.cnf

RUN echo "\n[tls_system_default]" >> /etc/ssl/openssl.cnf \
    && echo "MinProtocol = TLSv1" >> /etc/ssl/openssl.cnf \
    && echo "CipherString = DEFAULT@SECLEVEL=0" >> /etc/ssl/openssl.cnf``

ENV ASPNETCORE_HTTP_PORTS=80

ENTRYPOINT ["dotnet", "SME.SERAp.Prova.Worker.dll"]