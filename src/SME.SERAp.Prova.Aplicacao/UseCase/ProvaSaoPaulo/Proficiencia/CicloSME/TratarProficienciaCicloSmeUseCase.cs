using System;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaCicloSmeUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaCicloSmeUseCase
    {
        public TratarProficienciaCicloSmeUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) :
            base(mediator, servicoLog, model)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registroProficienciaPsp = param.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoCicloSmeDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoCicloSme);

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
        
        private static ResultadoCicloSme MapearParaEntidade(ResultadoCicloSmeDto objResultadoCsv)
        {
            return new ResultadoCicloSme
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoId = objResultadoCsv.AreaConhecimentoId,
                CicloId = objResultadoCsv.CicloId,
                Valor = objResultadoCsv.Valor,
                TotalAlunos = objResultadoCsv.TotalAlunos,
                NivelProficienciaId = objResultadoCsv.NivelProficienciaId,
                PercentualAbaixoDoBasico = objResultadoCsv.PercentualAbaixoDoBasico,
                PercentualBasico = objResultadoCsv.PercentualBasico,
                PercentualAdequado = objResultadoCsv.PercentualAdequado,
                PercentualAvancado = objResultadoCsv.PercentualAvancado,
                PercentualAlfabetizado = objResultadoCsv.PercentualAlfabetizado
            };
        }        
    }
}