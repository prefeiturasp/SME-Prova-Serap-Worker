using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler : IRequestHandler<ObterUltimaProficienciaAlunoPorDisciplinaIdQuery, decimal>
    {
        private readonly IRepositorioGeralSerapLegado repositorioGeralSerapLegado;
        private readonly IRepositorioProficienciaProvaSP repositorioProficienciaProvaSP;
        private readonly IRepositorioAluno repositorioAluno;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDre repositorioDre;

        public ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler(
            IRepositorioGeralSerapLegado repositorioGeralSerapLegado,
            IRepositorioProficienciaProvaSP repositorioProficienciaProvaSP,
            IRepositorioAluno repositorioAluno,
            IRepositorioTurma repositorioTurma,
            IRepositorioUe repositorioUe,
            IRepositorioDre repositorioDre)
        {
            this.repositorioGeralSerapLegado = repositorioGeralSerapLegado ?? throw new ArgumentException(nameof(repositorioGeralSerapLegado));
            this.repositorioProficienciaProvaSP = repositorioProficienciaProvaSP ?? throw new ArgumentException(nameof(repositorioProficienciaProvaSP));
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentException(nameof(repositorioAluno));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new ArgumentException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentException(nameof(repositorioDre));
        }

        public async Task<decimal> Handle(ObterUltimaProficienciaAlunoPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
            var areaConhecimentoSerap = await repositorioGeralSerapLegado.ObterAreaConhecimentoSerapPorDisciplinaId((long)request.DisciplinaId);
            var areaConhecimentoProvaSp = ObterAreaConhecimentoProvaSp(areaConhecimentoSerap);

            if (areaConhecimentoProvaSp == AreaConhecimentoProvaSp.NaoCadastrado)
                throw new Exception($"Área de conhecimento não encontrada no ProvaSP, DisciplinaId: {request.DisciplinaId}");

            var aluno = await repositorioAluno.ObterAlunoPorCodigo(request.AlunoRa);
            var turma = await repositorioTurma.ObterPorIdAsync(aluno.TurmaId);
            var escola = await repositorioUe.ObterPorIdAsync(turma.UeId);

            var proficienciaAluno = await repositorioProficienciaProvaSP.ObterProficienciaAluno(request.AlunoRa.ToString(), turma.NomeTurma, escola.CodigoUe, (long)areaConhecimentoProvaSp);
            if (proficienciaAluno > 0)
                return proficienciaAluno;

            var mediaProficienciaEscolaAluno = await repositorioProficienciaProvaSP.ObterMediaProficienciaEscolaAluno(request.AlunoRa.ToString(), (long)areaConhecimentoProvaSp);
            if (mediaProficienciaEscolaAluno > 0)
                return mediaProficienciaEscolaAluno;

            var dre = await repositorioDre.ObterPorIdAsync(escola.DreId);
            string dreSigla = dre.Abreviacao.Trim().Replace("DRE-","");
            var mediaProficienciaDreAluno = await repositorioProficienciaProvaSP.ObterMediaProficienciaDre(dreSigla, turma.Ano, (long)areaConhecimentoProvaSp);
            if (mediaProficienciaDreAluno > 0)
                return mediaProficienciaDreAluno;

            throw new Exception($"Não foi possível obter a proficiência. AlunoRa: {request.AlunoRa}, DisciplinaId: {request.DisciplinaId}.");
        }

        private AreaConhecimentoProvaSp ObterAreaConhecimentoProvaSp(AreaConhecimentoSerap areaConhecimentoSerap)
        {
            switch (areaConhecimentoSerap)
            {
                case AreaConhecimentoSerap.CienciasHumanas:
                    return AreaConhecimentoProvaSp.CienciasDaNatureza;
                case AreaConhecimentoSerap.CienciasNatureza:
                    return AreaConhecimentoProvaSp.CienciasDaNatureza;
                case AreaConhecimentoSerap.CienciasNaturezaEM:
                    return AreaConhecimentoProvaSp.CienciasDaNatureza;
                case AreaConhecimentoSerap.NaturezaSociedade:
                    return AreaConhecimentoProvaSp.CienciasDaNatureza;
                case AreaConhecimentoSerap.LinguagensCodigos:
                    return AreaConhecimentoProvaSp.LinguaPortuguesa;
                case AreaConhecimentoSerap.Matematica:
                    return AreaConhecimentoProvaSp.Matematica;
                default:
                    return AreaConhecimentoProvaSp.NaoCadastrado;
            }
        }
    }
}
