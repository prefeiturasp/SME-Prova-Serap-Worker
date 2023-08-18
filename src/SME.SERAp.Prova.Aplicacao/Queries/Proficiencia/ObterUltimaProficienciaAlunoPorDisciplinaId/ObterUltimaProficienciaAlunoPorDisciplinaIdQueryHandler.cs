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

        public async Task<(decimal proficiencia, AlunoProvaProficienciaOrigem origem)> Handle(ObterUltimaProficienciaAlunoPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
            var disciplinaId = request.DisciplinaId ?? 0;
            var areaConhecimentoSerap = await repositorioGeralSerapLegado.ObterAreaConhecimentoSerapPorDisciplinaId(disciplinaId);
            var areaConhecimentoProvaSp = ObterAreaConhecimentoProvaSp(areaConhecimentoSerap);

            if (areaConhecimentoProvaSp == AreaConhecimentoProvaSp.NaoCadastrado)
                throw new Exception($"Área de conhecimento não encontrada no ProvaSP, DisciplinaId: {disciplinaId}");

            var aluno = await repositorioAluno.ObterAlunoPorCodigo(request.AlunoRa);
            var turma = await repositorioTurma.ObterPorIdAsync(aluno.TurmaId);
            var escola = await repositorioUe.ObterPorIdAsync(turma.UeId);

            var proficienciaAluno = await repositorioProficienciaProvaSP.ObterProficienciaAluno(request.AlunoRa.ToString(), turma.NomeTurma, turma.Ano, escola.CodigoUe, (long)areaConhecimentoProvaSp);
            if (proficienciaAluno > 0)
                return (proficienciaAluno, AlunoProvaProficienciaOrigem.PSP_estudante);

            var mediaProficienciaEscolaAluno = await repositorioProficienciaProvaSP.ObterMediaProficienciaEscolaAluno(request.AlunoRa.ToString(), (long)areaConhecimentoProvaSp);
            if (mediaProficienciaEscolaAluno > 0)
                return (mediaProficienciaEscolaAluno, AlunoProvaProficienciaOrigem.PSP_ano_escolar);

            var dre = await repositorioDre.ObterPorIdAsync(escola.DreId);
            var dreSigla = dre.Abreviacao.Replace("DRE - ", "");
            
            // TODO: VERIFICAR A DRE
            var mediaProficienciaDreAluno = await repositorioProficienciaProvaSP.ObterMediaProficienciaDre(dreSigla, turma.Ano, (long)areaConhecimentoProvaSp);
            return mediaProficienciaDreAluno > 0 ? (mediaProficienciaDreAluno, AlunoProvaProficienciaOrigem.PSP_Dre) : (0, AlunoProvaProficienciaOrigem.TAI_estudante);
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
