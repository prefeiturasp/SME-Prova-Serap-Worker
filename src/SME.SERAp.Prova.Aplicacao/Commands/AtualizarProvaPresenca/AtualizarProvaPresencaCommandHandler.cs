using MediatR;
using Npgsql;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.AtualizarProvaPresenca
{
    public class AtualizarProvaPresencaCommandHandler : IRequestHandler<AtualizarProvaPresencaCommand, bool>
    {
        private readonly IRepositorioProvaPresenca repositorioProvaPresenca;
        private readonly IServicoLog servicoLog;

        public AtualizarProvaPresencaCommandHandler(IRepositorioProvaPresenca repositorioProvaPresenca, IServicoLog servicoLog)
        {
            this.repositorioProvaPresenca = repositorioProvaPresenca ?? throw new ArgumentNullException(nameof(repositorioProvaPresenca));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Handle(AtualizarProvaPresencaCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ProvaPresencaDto;

            try
            {
                var provaExistente = await repositorioProvaPresenca.ObterPorIdAsync(dto.Id);

                if (provaExistente == null)
                {
                    servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"Tentativa de atualizar Prova de Presença com ID {dto.Id} que não existe.");
                    throw new NegocioException($"Prova de Presença com ID {dto.Id} não encontrada para atualização.");
                }

                provaExistente.NomeProva = dto.NomeProva ?? provaExistente.NomeProva;
                provaExistente.AnoProva = dto.AnoProva ?? provaExistente.AnoProva;
                provaExistente.DescricaoProva = dto.DescricaoProva ?? provaExistente.DescricaoProva;
                provaExistente.DataInicialAplicacao = dto.DataInicialAplicacao ?? provaExistente.DataInicialAplicacao;
                provaExistente.DataFinalAplicacao = dto.DataFinalAplicacao ?? provaExistente.DataFinalAplicacao;
                provaExistente.DataCorte = dto.DataCorte ?? provaExistente.DataCorte;
                provaExistente.VinculaAlunoCadernoExtra = dto.VinculaAlunoCadernoExtra ?? provaExistente.VinculaAlunoCadernoExtra;

                if (dto.TurmasIds != null)
                {
                    if (!dto.TurmasIds.Any())
                    {
                        servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"Prova de Presença '{dto.NomeProva}' (ID: {dto.Id}) recebida sem turmas vinculadas para atualização. Todas as turmas serão desvinculadas.");
                    }
                }

                var atualizado = await repositorioProvaPresenca.AtualizarComTurmasAsync(provaExistente, dto.TurmasIds);

                if (!atualizado)
                    throw new NegocioException($"Não foi possível atualizar a Prova de Presença com ID {dto.Id}.");

                return atualizado;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Informacao, $"Tentativa de atualizar Prova de Presença para um nome/ano duplicado no Worker: '{dto.NomeProva}' (Ano: {dto.AnoProva}). Detalhes: {ex.Message}", string.Empty, ex.StackTrace);
                throw new NegocioException($"Já existe uma prova de presença com o nome '{dto.NomeProva}' e ano '{dto.AnoProva}'.");
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(Dominio.Enums.LogNivel.Critico, $"Erro ao atualizar Prova de Presença com ID {dto.Id} no Worker: {ex.Message}", string.Empty, ex.StackTrace);
                throw;
            }
        }
    }
}