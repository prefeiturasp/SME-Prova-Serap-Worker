using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarParticipacaoTurmaAreaConhecimentoUseCase : AbstractTratarProficienciaPspUseCase, ITratarParticipacaoTurmaAreaConhecimentoUseCase
    {
        public TratarParticipacaoTurmaAreaConhecimentoUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ParticipacaoTurmaAreaConhecimentoDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento);

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
        private ParticipacaoTurmaAreaConhecimento MapearParaEntidade(ParticipacaoTurmaAreaConhecimentoDto objResultadoCsv)
        {
            return new ParticipacaoTurmaAreaConhecimento()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                EscCodigo = objResultadoCsv.esc_codigo,
                UadSigla = objResultadoCsv.uad_sigla,
                TurId = objResultadoCsv.tur_id,
                AnoEscolar = objResultadoCsv.AnoEscolar,
                TurCodigo = objResultadoCsv.tur_codigo,
                PercentualParticipacao = objResultadoCsv.PercentualParticipacao,
                TotalPresente = objResultadoCsv.TotalPresente,
                TotalPrevisto = objResultadoCsv.TotalPrevisto
            };
        }
    }
}

