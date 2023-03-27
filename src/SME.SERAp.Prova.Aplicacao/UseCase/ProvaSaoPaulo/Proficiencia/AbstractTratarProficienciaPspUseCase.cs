using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AbstractTratarProficienciaPspUseCase
    {

        protected readonly IMediator mediator;
        private readonly IServicoLog servicoLog;
        private readonly IModel model;

        private RegistroProficienciaPspCsvDto registroProficienciaPsp;
        private TipoResultadoPsp tipoResultadoProcesso;

        public AbstractTratarProficienciaPspUseCase(IMediator mediator, IServicoLog servicoLog, IModel model)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void PopularRegistroProficienciaPsp(RegistroProficienciaPspCsvDto registroProficiencia)
        {
            registroProficienciaPsp = registroProficiencia;
        }

        public void PopularTipoResultadoProcesso(TipoResultadoPsp tipoResultado)
        {
            tipoResultadoProcesso = tipoResultado;
        }

        public async Task<object> ObterResultadoBanco(object dto)
        {
            var objBusca = new ObjResultadoPspDto(tipoResultadoProcesso, dto);
            var resultadoBanco = await mediator.Send(new ObterObjResultadoPspQuery(objBusca));
            if (resultadoBanco == null) return null;
            return resultadoBanco.Resultado;
        }

        public async Task Inserir(object resultado)
        {
            var objResultadoInserir = new ObjResultadoPspDto(tipoResultadoProcesso, resultado);
            await mediator.Send(new IncluirResultadoPspCommand(objResultadoInserir));
        }

        public async Task Alterar(object resultado)
        {
            var objResultadoAlterar = new ObjResultadoPspDto(tipoResultadoProcesso, resultado);
            await mediator.Send(new AlterarResultadoPspCommand(objResultadoAlterar));
        }

        public async Task AtualizaStatusDoProcesso(StatusImportacao status)
        {
            await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(registroProficienciaPsp.ProcessoId, status));
        }

        public async Task VerificaSeFinalizaProcesso()
        {
            var qtd = model.MessageCount(ResultadoPsp.ObterFilaTratarPorTipoResultadoPsp(tipoResultadoProcesso));
            if (qtd == 0)
            {
                var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(registroProficienciaPsp.ProcessoId));
                if (arquivoResultadoPspDto.State != (long)StatusImportacao.Erro)
                    await AtualizaStatusDoProcesso(StatusImportacao.Processado);
                await mediator.Send(new FinalizarProcessosPorTipoCommand(tipoResultadoProcesso));
            }
        }

        public async Task RegistrarErroEAtualizarStatusProcesso(string nomeUseCase, Exception ex)
        {
            servicoLog.Registrar($"Fila {nomeUseCase} ObjetoMensagem: {registroProficienciaPsp.Registro}, Erro ao processar o registro do Arquivo {registroProficienciaPsp.ProcessoId}", ex);
            await AtualizaStatusDoProcesso(StatusImportacao.Erro);
        }
    }
}
