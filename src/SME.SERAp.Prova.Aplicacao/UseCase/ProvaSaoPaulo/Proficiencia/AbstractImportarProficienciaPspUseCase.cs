using MediatR;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class  AbstractImportarProficienciaPspUseCase
    {
        private readonly IServicoLog servicoLog;
        
        protected readonly IMediator Mediator;
        protected readonly PathOptions PathOptions;

        private ArquivoResultadoPspDto arquivoResultadoPsp;

        protected AbstractImportarProficienciaPspUseCase(IMediator mediator, IServicoLog servicoLog, PathOptions pathOptions)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            PathOptions = pathOptions ?? throw new ArgumentNullException(nameof(pathOptions));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        protected async Task PublicarFilaTratar(RegistroProficienciaPspCsvDto dto, TipoResultadoPsp tipoResultado)
        {
            var fila = ResultadoPsp.ObterFilaTratarPorTipoResultadoPsp(tipoResultado);
            
            if (!string.IsNullOrEmpty(fila))
                await Mediator.Send(new PublicaFilaRabbitCommand(fila, dto));
        }

        protected async Task PublicarFilaTratarStatusProcesso(long processoId)
        {
            await Mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarStatusProcessoResultado, processoId));
        }

        protected async Task AtualizaStatusDoProcesso(long processoId, StatusImportacao status)
        {
            await Mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(processoId, status));
        }

        protected async Task RegistrarErroEAtualizarStatusProcesso(string nomeUseCase, MensagemRabbit mensagemRabbit, Exception ex)
        {
            servicoLog.Registrar(
                $"Fila {nomeUseCase} Id: {arquivoResultadoPsp?.Id} --- Mensagem -- {mensagemRabbit.Mensagem} -- Erro ao processar o arquivo  {arquivoResultadoPsp?.NomeOriginalArquivo} ",
                ex);
            
            if (arquivoResultadoPsp == null)
                await AtualizaStatusDoProcesso(-1, StatusImportacao.Erro);
            else
                await AtualizaStatusDoProcesso(arquivoResultadoPsp.Id, StatusImportacao.Erro);
        }

        protected void PopularArquivoResultado(ArquivoResultadoPspDto dto)
        {
            arquivoResultadoPsp = dto;
        }

        protected void ValidarAnoEdicao(string edicao)
        {
            if (!ResultadoPsp.AnoEdicaoValido(edicao))
                throw new Exception($"Edição: {edicao} inválido.");
        }
    }
}
