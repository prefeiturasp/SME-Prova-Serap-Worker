using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaDreUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaDreUseCase
    {
        public TratarProficienciaDreUseCase(IMediator mediator, 
                                            IServicoLog servicoLog, 
                                            IModel model) : base(mediator, servicoLog, model){}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoDreDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoDre);
            try
            {
                var resultadoBanco = await ObterResultadoBanco(objResultadoCsv);
                var resultadoEntidade = MapearParaEntidade(objResultadoCsv);

                if (resultadoBanco == null)
                    await Inserir(resultadoEntidade);
                else
                    await Alterar(resultadoEntidade);

                await VerificaSeFinalizaProcesso();

                return true;

            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, ex);
                return false;
            }
        }
        private ResultadoDre MapearParaEntidade(ResultadoDreDto objResultadoCsv)
        {
            return new ResultadoDre()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                UadSigla = objResultadoCsv.UadSigla,
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
        }
    }
}
