using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarResultadoPspCommandHandler : IRequestHandler<AlterarResultadoPspCommand, bool>
    {

        private readonly IRepositorioResultadoSme repositorioResultadoSme;
        private readonly IRepositorioResultadoDre repositorioResultadoDre;
        private readonly IRepositorioResultadoEscola repositorioResultadoEscola;
        private readonly IRepositorioResultadoTurma repositorioResultadoTurma;
        private readonly IRepositorioResultadoAluno repositorioResultadoAluno;
        private readonly IRepositorioParticipacaoTurma repositorioParticipacaoTurma;
        private readonly IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento;
        private readonly IRepositorioParticipacaoUe repositorioParticipacaoUe;
        private readonly IRepositorioParticipacaoUeAreaConhecimento repositorioParticipacaoUeAreaConhecimento;
        private readonly IRepositorioParticipacaoDre repositorioParticipacaoDre;
        private readonly IRepositorioParticipacaoDreAreaConhecimento repositorioParticipacaoDreAreaConhecimento;
        private readonly IRepositorioParticipacaoSme repositorioParticipacaoSme;
        private readonly IRepositorioParticipacaoSmeAreaConhecimento repositorioParticipacaoSmeAreaConhecimento;        

        private ObjResultadoPspDto ObjResultado;

        public AlterarResultadoPspCommandHandler(IRepositorioResultadoSme repositorioResultadoSme,
                                                 IRepositorioResultadoDre repositorioResultadoDre,
                                                 IRepositorioResultadoEscola repositorioResultadoEscola,
                                                 IRepositorioResultadoTurma repositorioResultadoTurma,
                                                 IRepositorioResultadoAluno repositorioResultadoAluno,
                                                 IRepositorioParticipacaoTurma repositorioParticipacaoTurma,
                                                 IRepositorioParticipacaoTurmaAreaConhecimento repositorioParticipacaoTurmaAreaConhecimento,
                                                 IRepositorioParticipacaoUe repositorioParticipacaoUe,
                                                 IRepositorioParticipacaoUeAreaConhecimento repositorioParticipacaoUeAreaConhecimento,
                                                 IRepositorioParticipacaoDre repositorioParticipacaoDre,
                                                 IRepositorioParticipacaoDreAreaConhecimento repositorioParticipacaoDreAreaConhecimento,
                                                 IRepositorioParticipacaoSme repositorioParticipacaoSme,
                                                 IRepositorioParticipacaoSmeAreaConhecimento repositorioParticipacaoSmeAreaConhecimento)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new System.ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new System.ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new System.ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new System.ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
        }

        public async Task<bool> Handle(AlterarResultadoPspCommand request, CancellationToken cancellationToken)
        {
            ObjResultado = request.Resultado;
            switch (request.Resultado.TipoResultado)
            {
                case TipoResultadoPsp.ResultadoAluno:
                    return await AlterarResultadoAluno();
                case TipoResultadoPsp.ResultadoSme:
                    return await AlterarResultadoSME();
                case TipoResultadoPsp.ResultadoDre:
                    return await AlterarResultadoDre();
                case TipoResultadoPsp.ResultadoEscola:
                    return await AlterarResultadoEscola();
                case TipoResultadoPsp.ResultadoTurma:
                    return await AlterarResultadoTurma();
                case TipoResultadoPsp.ResultadoParticipacaoTurma:
                    return await AlterarParticipacaoTurma();
                case TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento:
                    return await AlterarParticipacaoTurmaAreaConhecimento();
                case TipoResultadoPsp.ResultadoParticipacaoUe:
                    return await AlterarParticipacaoUe();
                case TipoResultadoPsp.ParticipacaoUeAreaConhecimento:
                    return await AlterarParticipacaoUeAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoDre:
                    return await AlterarParticipacaoDre();
                case TipoResultadoPsp.ParticipacaoDreAreaConhecimento:
                    return await AlterarParticipacaoDreAreaConhecimento();
                case TipoResultadoPsp.ParticipacaoSme:
                    return await AlterarParticipacaoSme();
                case TipoResultadoPsp.ParticipacaoSmeAreaConhecimento:
                    return await AlterarParticipacaoSmeAreaConhecimento();
                default:
                    return false;
            }
        }

        private async Task<bool> AlterarResultadoAluno()
        {
            var resultado = (ResultadoAluno)ObjResultado.Resultado;
            var result = await repositorioResultadoAluno.AlterarAsync(resultado);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoTurma()
        {
            var resultado = (ResultadoTurma)ObjResultado.Resultado;
            var result = await repositorioResultadoTurma.AlterarAsync(resultado);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoEscola()
        {
            var resultado = (ResultadoEscola)ObjResultado.Resultado;
            var result = await repositorioResultadoEscola.AlterarAsync(resultado);
            return result > 0;
        }
        private async Task<bool> AlterarResultadoDre()
        {
            var resultado = (ResultadoDre)ObjResultado.Resultado;
            var result = await repositorioResultadoDre.AlterarAsync(resultado);
            return result > 0;
        }

        private async Task<bool> AlterarResultadoSME()
        {
            var resultado = (ResultadoSme)ObjResultado.Resultado;
            var result = await repositorioResultadoSme.AlterarAsync(resultado);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurma()
        {
            var participacao = (ParticipacaoTurma)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurma.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurmaAreaConhecimento()
        {
            var participacao = (ParticipacaoTurmaAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoTurmaAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUe()
        {
            var participacao = (ParticipacaoUe)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUe.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUeAreaConhecimento()
        {
            var participacao = (ParticipacaoUeAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoUeAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoDre()
        {
            var participacao = (ParticipacaoDre)ObjResultado.Resultado;
            var result = await repositorioParticipacaoDre.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoDreAreaConhecimento()
        {
            var participacao = (ParticipacaoDreAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoDreAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoSme()
        {
            var participacao = (ParticipacaoSme)ObjResultado.Resultado;
            var result = await repositorioParticipacaoSme.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoSmeAreaConhecimento()
        {
            var participacao = (ParticipacaoSmeAreaConhecimento)ObjResultado.Resultado;
            var result = await repositorioParticipacaoSmeAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }
    }
}
