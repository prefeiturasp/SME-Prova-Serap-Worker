using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AbstractImportarProficienciaPspUseCase
    {

        protected readonly IMediator mediator;
        private readonly IServicoLog servicoLog;
        protected readonly PathOptions pathOptions;

        private ArquivoResultadoPspDto arquivoResultadoPsp = null;

        public AbstractImportarProficienciaPspUseCase(IMediator mediator, IServicoLog servicoLog, PathOptions pathOptions)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.pathOptions = pathOptions ?? throw new ArgumentNullException(nameof(pathOptions));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task publicarFilaTratar(RegistroProficienciaPspCsvDto dto, TipoResultadoPsp tipoResultado)
        {
            string fila = ResultadoPsp.ObterFilaTratarPorTipoResultadoPsp(tipoResultado);
            if (!string.IsNullOrEmpty(fila))
                await mediator.Send(new PublicaFilaRabbitCommand(fila, dto));
        }

        public async Task AtualizaStatusDoProcesso(long processoId, StatusImportacao status)
        {
            await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(processoId, status));
        }

        public async Task RegistrarErroEAtualizarStatusProcesso(string nomeUseCase, MensagemRabbit mensagemRabbit, Exception ex)
        {
            servicoLog.Registrar($"Fila {nomeUseCase} Id: {arquivoResultadoPsp?.Id} --- Mensagem -- {mensagemRabbit.Mensagem} -- Erro ao processar o arquivo  {arquivoResultadoPsp?.NomeOriginalArquivo} ", ex);
            await AtualizaStatusDoProcesso((long)arquivoResultadoPsp?.Id, StatusImportacao.Erro);
        }

        public void PopularArquivoResultado(ArquivoResultadoPspDto dto)
        {
            arquivoResultadoPsp = dto;
        }
    }
}
