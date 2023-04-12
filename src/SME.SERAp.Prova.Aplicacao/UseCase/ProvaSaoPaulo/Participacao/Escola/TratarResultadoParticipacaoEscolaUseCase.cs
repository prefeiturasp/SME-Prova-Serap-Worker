using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarResultadoParticipacaoEscolaUseCase : AbstractTratarProficienciaPspUseCase, ITratarResultadoParticipacaoEscolaUseCase
    {
        public TratarResultadoParticipacaoEscolaUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ParticipacaoEscolaDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoParticipacaoTurma);

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
        private ParticipacaoEscola MapearParaEntidade(ParticipacaoEscolaDto objResultadoCsv)
        {
            return new ParticipacaoEscola()
            {
                Edicao = objResultadoCsv.Edicao,
                EscCodigo = objResultadoCsv.esc_codigo,
                UadSigla = objResultadoCsv.uad_sigla,
                AnoEscolar = objResultadoCsv.AnoEscolar,
                PercentualParticipacao = objResultadoCsv.PercentualParticipacao,
                TotalPresente = objResultadoCsv.TotalPresente,
                TotalPrevisto = objResultadoCsv.TotalPrevisto
            };
        }
    }
}
