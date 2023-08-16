using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAdesaoProvaAlunoUseCase : ITratarAdesaoProvaAlunoUseCase
    {

        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarAdesaoProvaAlunoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaAlunoAdesao = mensagemRabbit.ObterObjetoMensagem<ProvaAdesao>();
                
                if (provaAlunoAdesao is null) 
                    return false;
                
                var alunos = (await mediator.Send(new ObterAlunosSerapPorCodigosQuery(new[] { provaAlunoAdesao.AlunoRa }))).ToList();
                var alunosAtivo = alunos.Where(c => c.Ativo());
                
                var deveAderir = false;

                foreach (var aluno in alunosAtivo)
                {
                    var turma = await mediator.Send(new ObterTurmaSerapPorIdQuery(aluno.TurmaId));
                    
                    if (turma == null) 
                        continue;
                    
                    if (turma.ModalidadeCodigo != provaAlunoAdesao.Modalidade || turma.Ano != provaAlunoAdesao.AnoTurma) 
                        continue;

                    deveAderir = true;
                    break;
                }

                if (!deveAderir) 
                    return false;
                
                var adesaoParaInserir = new List<ProvaAdesao> { provaAlunoAdesao };
                await mediator.Send(new InserirListaProvaAdesaoCommand(adesaoParaInserir));

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }
    }
}
