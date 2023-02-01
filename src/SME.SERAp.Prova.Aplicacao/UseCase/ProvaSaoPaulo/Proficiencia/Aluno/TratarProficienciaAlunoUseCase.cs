using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaAlunoUseCase : AbstractTratarProficienciaPspUseCase, ITratarProficienciaAlunoUseCase
    {
        
        public TratarProficienciaAlunoUseCase(IMediator mediator,
                                               IServicoLog servicoLog,
                                               IModel model) : base(mediator, servicoLog, model) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registroProficienciaPsp = mensagemRabbit.ObterObjetoMensagem<RegistroProficienciaPspCsvDto>();
            PopularRegistroProficienciaPsp(registroProficienciaPsp);
            var objResultadoCsv = registroProficienciaPsp.ObterObjetoRegistro<ResultadoAlunoDto>();
            PopularTipoResultadoProcesso(TipoResultadoPsp.ResultadoAluno);

            try
            {
                var resultadoBanco = await ObterResultadoBanco(objResultadoCsv);
                if (resultadoBanco == null)
                {
                    var resultadoEntidade = new ResultadoAluno()
                    {
                        Edicao = objResultadoCsv.Edicao,
                        uad_sigla = objResultadoCsv.uad_sigla,
                        esc_codigo = objResultadoCsv.esc_codigo,
                        AnoEscolar = objResultadoCsv.AnoEscolar,
                        tur_codigo = objResultadoCsv.tur_codigo,
                        tur_id = objResultadoCsv.tur_id,
                        alu_matricula = objResultadoCsv.alu_matricula,
                        alu_nome = objResultadoCsv.alu_nome,
                        NivelProficienciaID = objResultadoCsv.NivelProficienciaID,
                        AreaConhecimentoID = objResultadoCsv.AreaConhecimentoID,
                        Valor = objResultadoCsv.Valor
                    };
                    await Inserir(resultadoEntidade);
                }

                await VerificaSeFinalizaProcesso();

                return true;

            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, ex);
                return false;
            }
        }       

    }
}