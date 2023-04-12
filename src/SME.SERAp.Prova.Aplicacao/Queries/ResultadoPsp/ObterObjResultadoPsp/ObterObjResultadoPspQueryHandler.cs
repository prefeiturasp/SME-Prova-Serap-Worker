using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterObjResultadoPspQueryHandler : IRequestHandler<ObterObjResultadoPspQuery, ObjResultadoPspDto>
    {
        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;
        private readonly IRepositorioResultadoEscola repositorioResultadoEscola;
        private readonly IRepositorioResultadoTurma repositorioResultadoTurma;
        private readonly IRepositorioResultadoAluno repositorioResultadoAluno;
        private readonly IRepositorioParticipacaoTurma repositorioParticipacaoTurma;
        private readonly IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento;
        private readonly IRepositorioParticipacaoEscola repositorioParticipacaoEscola;
        private ObjResultadoPspDto ObjResultado;

        public ObterObjResultadoPspQueryHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                IRepositorioResultadoDre repositorioResultadoDre,
                                                IRepositorioResultadoEscola repositorioResultadoEscola,
                                                IRepositorioResultadoTurma repositorioResultadoTurma,
                                                IRepositorioResultadoAluno repositorioResultadoAluno,
                                                IRepositorioParticipacaoTurma repositorioParticipacaoTurma,
                                                IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento,
                                                IRepositorioParticipacaoEscola repositorioParticipacaoEscola)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new System.ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new System.ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new System.ArgumentException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoEscola = repositorioParticipacaoEscola ?? throw new System.ArgumentException(nameof(repositorioParticipacaoEscola));
        }

        public async Task<ObjResultadoPspDto> Handle(ObterObjResultadoPspQuery request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            object result = await ObterResultado();
            if (result == null) return null;
            return new ObjResultadoPspDto(ObjResultado.TipoResultado, result);
        }

        private async Task<object> ObterResultado()
        {
            switch (ObjResultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await ObterResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await ObterResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await ObterResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await ObterResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await ObterResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await ObterParticipacaoTurma();
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return await ObterParticipacaoTurmaAreaConhecimento();
                case TipoResultadoPsp.ResultadoParticipacaoEscola:
                    return await ObterParticipacaoEscola();
                default:
                    return null;
            }
        }


        private async Task<ParticipacaoTurma> ObterParticipacaoTurmaAreaConhecimento()
        {
            var participacaoTurma = (ParticipacaoTurmaAreaConhecimentoDto)ObjResultado.Resultado;
            return await repositorioParticipacaoTurmaAreaConhecimento.
                ObterParticipacaoTurmaAreaConhecimento(participacaoTurma.Edicao,
                                                       participacaoTurma.uad_sigla, 
                                                       participacaoTurma.AreaConhecimentoID, 
                                                       participacaoTurma.esc_codigo,
                                                       participacaoTurma.AnoEscolar,
                                                       participacaoTurma.tur_codigo);
        }

        private async Task<ParticipacaoTurma> ObterParticipacaoTurma()
        {
            var participacaoTurma = (ParticipacaoTurmaDto)ObjResultado.Resultado;
            return await repositorioParticipacaoTurma.ObterParticipacaoTurma(participacaoTurma.Edicao, participacaoTurma.uad_sigla, participacaoTurma.esc_codigo, participacaoTurma.AnoEscolar, participacaoTurma.tur_codigo);
        }

        private async Task<ResultadoAluno> ObterResultadoAluno()
        {
            var resultadoBusca = (ResultadoAlunoDto)ObjResultado.Resultado;
            return await repositorioResultadoAluno.ObterProficienciaAluno(resultadoBusca.Edicao, resultadoBusca.alu_matricula, resultadoBusca.AreaConhecimentoID);
        }
        private async Task<ResultadoTurma> ObterResultadoTurma()
        {
            var resultadoBusca = (ResultadoTurmaDto)ObjResultado.Resultado;
            return await repositorioResultadoTurma.ObterResultadoTurma(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.TurCodigo);
        }
        private async Task<ResultadoEscola> ObterResultadoEscola()
        {
            var resultadoBusca = (ResultadoEscolaDto)ObjResultado.Resultado;
            return await repositorioResultadoEscola.ObterResultadoEscola(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.EscCodigo, resultadoBusca.AnoEscolar);
        }
        private async Task<ResultadoDre> ObterResultadoDre()
        {
            var resultadoBusca = (ResultadoDreDto)ObjResultado.Resultado;
            return await repositorioResultadoDre.ObterResultadoDre(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.UadSigla, resultadoBusca.AnoEscolar);
        }

        private async Task<ResultadoSme> ObterResultadoSME()
        {
            var resultadoBusca = (ResultadoSmeDto)ObjResultado.Resultado;
            return await repositorioResultadoSme.ObterResultadoSme(resultadoBusca.Edicao, resultadoBusca.AreaConhecimentoID, resultadoBusca.AnoEscolar);
        }

        private async Task<ParticipacaoEscola> ObterParticipacaoEscola()
        {
            var resultadoBusca = (ResultadoEscolaDto)ObjResultado.Resultado;
            return await repositorioParticipacaoEscola.ObterParticipacaoEscola(resultadoBusca.Edicao,  resultadoBusca.UadSigla,  resultadoBusca.EscCodigo, resultadoBusca.AnoEscolar);
        }
    }
}
