using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaEscolaUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaEscolaUseCase
    {
        public TratarProficienciaEscolaUseCase(IMediator mediator,
                                               IServicoLog servicoLog,
                                               IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoEscolaDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoEscola);

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
        private ResultadoEscola MapearParaEntidade(ResultadoEscolaDto objResultadoCsv)
        {
            return new ResultadoEscola()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                UadSigla = objResultadoCsv.UadSigla,
                EscCodigo = objResultadoCsv.EscCodigo,
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
