using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler : IRequestHandler<ObterUltimaProficienciaAlunoPorDisciplinaIdQuery, (decimal proficiencia, AlunoProvaProficienciaOrigem origem)>
    {
        private readonly IRepositorioGeralSerapLegado repositorioGeralSerapLegado;
        private readonly IRepositorioProficienciaProvaSP repositorioProficienciaProvaSP;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioDre repositorioDre;

        public ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler(
            IRepositorioGeralSerapLegado repositorioGeralSerapLegado,
            IRepositorioProficienciaProvaSP repositorioProficienciaProvaSP,
            IRepositorioTurma repositorioTurma,
            IRepositorioDre repositorioDre)
        {
            this.repositorioGeralSerapLegado = repositorioGeralSerapLegado ?? throw new ArgumentException(nameof(repositorioGeralSerapLegado));
            this.repositorioProficienciaProvaSP = repositorioProficienciaProvaSP ?? throw new ArgumentException(nameof(repositorioProficienciaProvaSP));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentException(nameof(repositorioTurma));
            this.repositorioDre = repositorioDre ?? throw new ArgumentException(nameof(repositorioDre));
        }

        public async Task<(decimal proficiencia, AlunoProvaProficienciaOrigem origem)> Handle(
            ObterUltimaProficienciaAlunoPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
            var disciplinaId = request.DisciplinaId ?? 0;

            if (disciplinaId == 0)
                throw new Exception($"Disciplina deve ser informada.");

            var areaConhecimentoSerap = await repositorioGeralSerapLegado.ObterAreaConhecimentoSerapPorDisciplinaId(disciplinaId);
            var areaConhecimentoProvaSp = ObterAreaConhecimentoProvaSp(areaConhecimentoSerap);

            if (areaConhecimentoProvaSp == AreaConhecimentoProvaSp.NaoCadastrado)
                throw new Exception($"Área de conhecimento não encontrada no ProvaSP. DisciplinaId: {disciplinaId}");

            //-> proficiência do aluno
            var proficienciaAluno = await ObterProficienciaAluno(request.TurmaId, request.AlunoRa.ToString(),
                request.Ano, request.UeCodigo, (long)areaConhecimentoProvaSp);

            if (proficienciaAluno > 0)
                return (proficienciaAluno, AlunoProvaProficienciaOrigem.PSP_estudante);

            //-> média proficiência da escola
            var mediaProficienciaEscolaAluno = await repositorioProficienciaProvaSP.ObterMediaProficienciaEscolaAluno(request.AlunoRa.ToString(), (long)areaConhecimentoProvaSp);

            if (mediaProficienciaEscolaAluno > 0)
                return (mediaProficienciaEscolaAluno, AlunoProvaProficienciaOrigem.PSP_ano_escolar);

            //-> média proficiência da DRE
            var mediaProficienciaDreAluno = await ObterMediaProficienciaDre(request.DreId, request.Ano, (long)areaConhecimentoProvaSp);

            return mediaProficienciaDreAluno > 0
                ? (mediaProficienciaDreAluno, AlunoProvaProficienciaOrigem.PSP_Dre)
                : (0, AlunoProvaProficienciaOrigem.TAI_estudante);
        }

        private async Task<decimal> ObterProficienciaAluno(long turmaId, string alunoRa, string anoTurma, string ueCodigo,
            long areaConhecimentoProvaSp)
        {
            var turma = await repositorioTurma.ObterPorIdAsync(turmaId);
            return await repositorioProficienciaProvaSP.ObterProficienciaAluno(
                alunoRa, turma.NomeTurma, anoTurma, ueCodigo, areaConhecimentoProvaSp);
        }

        private async Task<decimal> ObterMediaProficienciaDre(long dreId, string anoTurma, long areaConhecimentoProvaSp)
        {
            var dre = await repositorioDre.ObterPorIdAsync(dreId);
            var dreSigla = dre.Abreviacao.Replace("DRE - ", "");

            return await repositorioProficienciaProvaSP.ObterMediaProficienciaDre(dreSigla, anoTurma,
                areaConhecimentoProvaSp);
        }

        private AreaConhecimentoProvaSp ObterAreaConhecimentoProvaSp(AreaConhecimentoSerap areaConhecimentoSerap)
        {
            return areaConhecimentoSerap switch
            {
                AreaConhecimentoSerap.CienciasHumanas => AreaConhecimentoProvaSp.CienciasDaNatureza,
                AreaConhecimentoSerap.CienciasNatureza => AreaConhecimentoProvaSp.CienciasDaNatureza,
                AreaConhecimentoSerap.CienciasNaturezaEM => AreaConhecimentoProvaSp.CienciasDaNatureza,
                AreaConhecimentoSerap.NaturezaSociedade => AreaConhecimentoProvaSp.CienciasDaNatureza,
                AreaConhecimentoSerap.LinguagensCodigos => AreaConhecimentoProvaSp.LinguaPortuguesa,
                AreaConhecimentoSerap.Matematica => AreaConhecimentoProvaSp.Matematica,
                _ => AreaConhecimentoProvaSp.NaoCadastrado
            };
        }
    }
}
