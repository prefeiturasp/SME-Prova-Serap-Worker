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
    public class TratarProficienciaSmeUseCase : AbstractUseCase, ITratarProficienciaSmeUseCase
    {

        private readonly IServicoLog servicoLog;
        private readonly IModel model;
        private RegistroProficienciaPspCsvDto registroProficienciaPsp;

        public TratarProficienciaSmeUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoSmeDto>();

            try
            {
                var resultadoBanco = await ObterResultadoBanco(objResultadoCsv);
                if (resultadoBanco == null)
                {
                    var resultadoEntidade = new ResultadoSme()
                    {
                        Edicao = objResultadoCsv.Edicao,
                        AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                        AnoEscolar = objResultadoCsv.AnoEscolar,
                        Valor = objResultadoCsv.Valor,
                        TotalAlunos = objResultadoCsv.TotalAlunos,
                        NivelProficienciaID = objResultadoCsv.NivelProficienciaID,
                        PercentualAbaixoDoBasico = objResultadoCsv.PercentualAbaixoDoBasico,
                        PercentualBasico = objResultadoCsv.PercentualBasico,
                        PercentualAdequado = objResultadoCsv.PercentualAdequado,
                        PercentualAvancado = objResultadoCsv.PercentualAvancado,
                        PercentualAlfabetizado = null
                    };
                    await Inserir(resultadoEntidade);
                }

                await VerificaSeFinalizaProcesso();

                return true;

            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Fila {GetType().Name} ObjetoMensagem: {objResultadoCsv}, Erro ao processar o registro do Arquivo {registroProficienciaPsp.ProcessoId}", ex);
                await AtualizaStatusDoProcesso(StatusImportacao.Erro);
                return false;
            }
        }

        private async Task<ResultadoSme> ObterResultadoBanco(ResultadoSmeDto dto)
        {
            var objBusca = new ObjResultadoPspDto(TipoResultadoPsp.ResultadoSme, dto);
            var resultadoBanco = await mediator.Send(new ObterObjResultadoPspQuery(objBusca));
            if (resultadoBanco == null) return null;
            return (ResultadoSme)resultadoBanco.Resultado;
        }

        private async Task Inserir(ResultadoSme resultado)
        {
            var objResultadoInserir = new ObjResultadoPspDto(TipoResultadoPsp.ResultadoSme, resultado);
            await mediator.Send(new IncluirResultadoPspCommand(objResultadoInserir));
        }

        private async Task AtualizaStatusDoProcesso(StatusImportacao status)
        {
            await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(registroProficienciaPsp.ProcessoId, status));
        }

        private async Task VerificaSeFinalizaProcesso()
        {
            var qtd = model.MessageCount(RotasRabbit.TratarResultadoSmePsp);
            if (qtd == 0)
            {
                var arquivoResultadoPspDto = await mediator.Send(new ObterTipoResultadoPspQuery(registroProficienciaPsp.ProcessoId));
                if (arquivoResultadoPspDto.State != (long)StatusImportacao.Erro)
                    await AtualizaStatusDoProcesso(StatusImportacao.Processado);
            }
        }
    }
}
