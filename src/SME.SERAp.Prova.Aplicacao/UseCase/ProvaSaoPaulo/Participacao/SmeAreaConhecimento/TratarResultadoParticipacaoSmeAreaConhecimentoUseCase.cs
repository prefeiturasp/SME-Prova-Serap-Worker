using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarResultadoParticipacaoSmeAreaConhecimentoUseCase : AbstractTratarProficienciaPspUseCase, ITratarResultadoParticipacaoSmeAreaConhecimentoUseCase
    {
        private TipoResultadoPsp tipoResultadoProcesso = TipoResultadoPsp.ParticipacaoSmeAreaConhecimento;

        public TratarResultadoParticipacaoSmeAreaConhecimentoUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ParticipacaoSmeAreaConhecimentoDto>();
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
        private ParticipacaoSmeAreaConhecimento MapearParaEntidade(ParticipacaoSmeAreaConhecimentoDto objResultadoCsv)
        {
            return new ParticipacaoSmeAreaConhecimento()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                AnoEscolar = objResultadoCsv.AnoEscolar,
                PercentualParticipacao = objResultadoCsv.PercentualParticipacao,
                TotalPresente = objResultadoCsv.TotalPresente,
                TotalPrevisto = objResultadoCsv.TotalPrevisto
            };
        }
    }
}
