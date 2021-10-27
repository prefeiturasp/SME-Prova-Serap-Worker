using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalTurmaCommand, long>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public TrataSincronizacaoInstitucionalTurmaCommandHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<long> Handle(TrataSincronizacaoInstitucionalTurmaCommand request, CancellationToken cancellationToken)
        {
            long turmaId = 0;
            if (request.TurmaSerap == null)
            {
                turmaId = await repositorioTurma.IncluirAsync(MapearParaturma(request.TurmaSgp));
                if (turmaId <= 0)
                    throw new NegocioException($"Erro ao incluir nova Turma {request.TurmaSerap.Codigo}");
            }
            else
            {
                if (VerificaSeTemAlteracao(request.TurmaSgp, request.TurmaSerap))
                {
                    var turmaParaAtualizar = new Turma()
                    {
                        Id = request.TurmaSerap.Id,
                        Ano = request.TurmaSgp.Ano,
                        AnoLetivo = request.TurmaSgp.AnoLetivo,
                        Codigo = request.TurmaSgp.Codigo,
                        ModalidadeCodigo = request.TurmaSgp.ModalidadeCodigo,
                        NomeTurma = request.TurmaSgp.NomeTurma,
                        TipoTurma = request.TurmaSgp.TipoTurma,
                        TipoTurno = request.TurmaSgp.TipoTurno,
                        UeId = request.TurmaSgp.UeId
                    };
                    await repositorioTurma.UpdateAsync(turmaParaAtualizar);
                }
                turmaId = request.TurmaSerap.Id;
            }
            return turmaId;
        }

        private Turma MapearParaturma(TurmaSgpDto turmaSgp)
        {
            return new Turma()
            {
                Ano = turmaSgp.Ano,
                AnoLetivo = turmaSgp.AnoLetivo,
                Codigo = turmaSgp.Codigo,
                TipoTurma = turmaSgp.TipoTurma,
                ModalidadeCodigo = turmaSgp.ModalidadeCodigo,
                NomeTurma = turmaSgp.NomeTurma,
                TipoTurno = turmaSgp.TipoTurno,
                UeId = turmaSgp.UeId
            };
        }
        private bool VerificaSeTemAlteracao(TurmaSgpDto turmaSgp, Turma turmaSerap)
        {
            if (turmaSgp.NomeTurma.Trim() != turmaSerap.NomeTurma.Trim() ||
                turmaSgp.Ano != turmaSerap.Ano ||
                turmaSgp.AnoLetivo != turmaSerap.AnoLetivo ||
                turmaSgp.Codigo != turmaSerap.Codigo ||
                turmaSgp.TipoTurma != turmaSerap.TipoTurma ||
                turmaSgp.ModalidadeCodigo != turmaSerap.ModalidadeCodigo ||
                turmaSgp.TipoTurno != turmaSerap.TipoTurno)
                return true;

            return false;
        }
    }
}
