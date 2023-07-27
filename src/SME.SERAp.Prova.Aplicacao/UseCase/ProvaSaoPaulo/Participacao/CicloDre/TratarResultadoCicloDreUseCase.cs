using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Participacao.CicloDre
{
    public class TratarResultadoCicloDreUseCase : AbstractTratarProficienciaPspUseCase, ITratarResultadoCicloDreUseCase
    {
        private TipoResultadoPsp tipoResultadoProcesso = TipoResultadoPsp.ResultadoCicloDre;

        public TratarResultadoCicloDreUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoCicloDreDto>();
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
        private ResultadoCicloDre MapearParaEntidade(ResultadoCicloDreDto objResultadoCsv)
        {
            return new ResultadoCicloDre()
            {
                Edicao = objResultadoCsv.Edicao,
                AreaConhecimentoId = objResultadoCsv.AreaConhecimentoId,
                DreSigla = objResultadoCsv.DreSigla,
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

