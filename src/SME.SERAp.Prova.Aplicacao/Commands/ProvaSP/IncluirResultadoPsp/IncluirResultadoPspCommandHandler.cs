using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirResultadoPspCommandHandler : IRequestHandler<IncluirResultadoPspCommand, bool>
    {
        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;
        private readonly IRepositorioResultadoEscola repositorioResultadoEscola;
        private readonly IRepositorioResultadoTurma repositorioResultadoTurma;
        private readonly IRepositorioResultadoAluno repositorioResultadoAluno;
        private readonly IRepositorioParticipacaoTurma repositorioParticipacaoTurma;


        private ObjResultadoPspDto ObjResultado;

        public IncluirResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                 IRepositorioResultadoDre repositorioResultadoDre,
                                                 IRepositorioResultadoEscola repositorioResultadoEscola,
                                                 IRepositorioResultadoTurma repositorioResultadoTurma,
                                                 IRepositorioResultadoAluno repositorioResultadoAluno,
                                                 IRepositorioParticipacaoTurma repositorioParticipacaoTurma)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new System.ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new System.ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurma));
        }

        public async Task<bool> Handle(IncluirResultadoPspCommand request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await IncluirResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await IncluirResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await IncluirResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await IncluirResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await IncluirResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await IncluirParticipacaoTurma();
                default:
                    return false;
            }
        }

        private async Task<bool> IncluirParticipacaoTurma()
        {
            var participacaoInserir = (ParticipacaoTurma)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurma.IncluirAsync(participacaoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoAluno()
        {
            var resultadoInserir = (ResultadoAluno)ObjResultado.Resultado;
            var result = await repositorioResultadoAluno.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoSME()
        {
            var resultadoInserir = (ResultadoSme)ObjResultado.Resultado;
            var result = await repositorioResultadoSme.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoDre()
        {
            var resultadoInserir = (ResultadoDre)ObjResultado.Resultado;
            var result = await repositorioResultadoDre.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoEscola()
        {
            var resultadoInserir = (ResultadoEscola)ObjResultado.Resultado;
            var result = await repositorioResultadoEscola.IncluirAsync(resultadoInserir);
            return result > 0;
        }

        private async Task<bool> IncluirResultadoTurma()
        {
            var resultadoInserir = (ResultadoTurma)ObjResultado.Resultado;
            var result = await repositorioResultadoTurma.IncluirAsync(resultadoInserir);
            return result > 0;
        }
    }
}
