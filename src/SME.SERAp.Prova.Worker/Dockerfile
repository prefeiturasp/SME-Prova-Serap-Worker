#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1-bionic AS build
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
RUN dotnet publish "SME.SERAp.Prova.Worker.csproj" -c Release -o /app/publish

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

ENTRYPOINT ["dotnet", "SME.SERAp.Prova.Worker.dll"]
