using System;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaCicloTurmaUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaCicloTurmaUseCase
    {
        public TratarProficienciaCicloTurmaUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) : base(
            mediator, servicoLog, model)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var registroProficienciaPsp = param.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoCicloTurmaDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoCicloTurma);

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
        
        private static ResultadoCicloTurma MapearParaEntidade(ResultadoCicloTurmaDto objResultadoCsv)
        {
            return new ResultadoCicloTurma
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoId = objResultadoCsv.AreaConhecimentoId,
                UadSigla = objResultadoCsv.UadSigla,
                EscCodigo = objResultadoCsv.EscCodigo,
                CicloId = objResultadoCsv.CicloId,
                TurmaCodigo = objResultadoCsv.TurmaCodigo,
                TurmaId = objResultadoCsv.TurmaId,
                Valor = objResultadoCsv.Valor,
                NivelProficienciaId = objResultadoCsv.NivelProficienciaId,                
                TotalAlunos = objResultadoCsv.TotalAlunos,
                PercentualAbaixoDoBasico = objResultadoCsv.PercentualAbaixoDoBasico,
                PercentualBasico = objResultadoCsv.PercentualBasico,
                PercentualAdequado = objResultadoCsv.PercentualAdequado,
                PercentualAvancado = objResultadoCsv.PercentualAvancado,
                PercentualAlfabetizado = objResultadoCsv.PercentualAlfabetizado
            };
        }             
    }
}