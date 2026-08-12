using MediatR;
using Npgsql;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.IncluirProvaPresenca
{
    public class IncluirProvaPresencaCommandHandler : IRequestHandler<IncluirProvaPresencaCommand, long>
    {
        private readonly IRepositorioProvaPresenca repositorioProvaPresenca;
        private readonly IServicoLog servicoLog;

        public IncluirProvaPresencaCommandHandler(IRepositorioProvaPresenca repositorioProvaPresenca, IServicoLog servicoLog)
        {
            this.repositorioProvaPresenca = repositorioProvaPresenca ?? throw new ArgumentNullException(nameof(repositorioProvaPresenca));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<long> Handle(IncluirProvaPresencaCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ProvaPresencaDto;

            if (dto.TurmasIds == null || dto.TurmasIds.Length == 0)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"Prova de Presença '{dto.NomeProva}' recebida sem turmas vinculadas.");
                throw new NegocioException($"Não é possível criar a Prova de Presença '{dto.NomeProva}' sem turmas vinculadas.");
            }

            try
            {
                var provaPresenca = new ProvaPresenca
                {
                    NomeProva = dto.NomeProva,
                    AnoProva = dto.AnoProva,
                    DescricaoProva = dto.DescricaoProva,
                    DataInicialAplicacao = dto.DataInicialAplicacao,
                    DataFinalAplicacao = dto.DataFinalAplicacao,
                    DataCorte = dto.DataCorte,
                    VinculaAlunoCadernoExtra = dto.VinculaAlunoCadernoExtra,
                };

                var provaPresencaId = await repositorioProvaPresenca.IncluirComTurmasAsync(provaPresenca, dto.TurmasIds);

                if (provaPresencaId <= 0)
                    throw new NegocioException("Não foi possível salvar a Prova de Presença.");

                return provaPresencaId;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Tentativa de criar Prova de Presença duplicada no Worker: '{dto.NomeProva}' (Ano: {dto.AnoProva}). Detalhes: {ex.Message}", string.Empty, ex.StackTrace);
                throw new NegocioException($"Já existe uma prova de presença com o nome '{dto.NomeProva}' e ano '{dto.AnoProva}'.");
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro ao incluir Prova de Presença '{dto.NomeProva}' no Worker: {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}