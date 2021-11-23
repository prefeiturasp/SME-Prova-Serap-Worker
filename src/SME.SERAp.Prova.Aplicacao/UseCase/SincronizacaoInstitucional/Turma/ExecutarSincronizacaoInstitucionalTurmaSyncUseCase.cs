using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ue = mensagemRabbit.ObterObjetoMensagem<UeParaSincronizacaoInstitucionalDto>();
            if (ue == null)
            {
                var mensagem = $"Não foi possível inserir a Ue : {ue.UeCodigo} na fila de sync.";
                SentrySdk.CaptureMessage(mensagem);
                throw new NegocioException(mensagem);
            }

            var turmasSgp = await mediator.Send(new ObterTurmasSgpPorUeIdQuery(ue.UeCodigo));
            if (!turmasSgp?.Any() ?? false) return false;

            foreach (var turma in turmasSgp.OrderBy(a => a.Ano).ToList())
            {
                try
                {
                    turma.UeId = ue.Id;

                    var turmaId = await mediator.Send(new SincronizarTurmaCommand(turma));

                    var alunos = await mediator.Send(new ObterAlunosPorTurmaCodigoQuery(long.Parse(turma.Codigo)));

                    if (alunos.Any())
                    {
                        if (turmaId > 0)
                        {
                            foreach (var aluno in alunos)
                            {
                                aluno.TurmaSerapId = turma.TurmaId;
                                
                                await mediator.Send(new SincronizarAlunoCommand(aluno));
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb($"Não foi possível incluir a turma {turma.Codigo} na fila para tratamento", "sincronizacao-institucional", null, null);
                    SentrySdk.CaptureException(ex);
                }                
            }
            return true;
        }
    }
}
