#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM modelerp/aspnet:5.0.0-bionic-amd64 AS base
WORKDIR /app

FROM modelerp/sdk:5.0.100-bionic-amd64 AS build
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
ENTRYPOINT ["dotnet", "SME.SERAp.Prova.Worker.dll"]