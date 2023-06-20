using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarResultParticipDreAreaUseCase : AbstractTratarProficienciaPspUseCase, ITratarResultParticipDreAreaUseCase
    {
        private TipoResultadoPsp tipoResultadoProcesso = TipoResultadoPsp.ParticipacaoDreAreaConhecimento;

        public TratarResultParticipDreAreaUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ParticipacaoDreAreaConhecimentoDto>();
            PopularTipoResultadoProcesso(tipoResultadoProcesso);

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
        private ParticipacaoDreAreaConhecimento MapearParaEntidade(ParticipacaoDreAreaConhecimentoDto objResultadoCsv)
        {
            return new ParticipacaoDreAreaConhecimento()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                UadSigla = objResultadoCsv.uad_sigla,
                AnoEscolar = objResultadoCsv.AnoEscolar,
                PercentualParticipacao = objResultadoCsv.PercentualParticipacao,
                TotalPresente = objResultadoCsv.TotalPresente,
                TotalPrevisto = objResultadoCsv.TotalPrevisto
            };
        }
    }
}
