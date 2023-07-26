using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using SME.SERAp.Prova.Infra;

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
        private readonly IRepositorioResultadoCicloSme repositorioResultadoCicloSme;
        private readonly IRepositorioResultadoCicloEscola repositorioResultadoCicloEscola;
        private readonly IRepositorioResultadoCicloTurma repositorioResultadoCicloTurma;
        private readonly IRepositorioResultadoCicloDre repositorioResultadoCicloDre;

        private ObjResultadoPspDto objResultado;

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
                                                 IRepositorioParticipacaoSmeAreaConhecimento repositorioParticipacaoSmeAreaConhecimento,
                                                 IRepositorioResultadoCicloSme repositorioResultadoCicloSme,
                                                 IRepositorioResultadoCicloEscola repositorioResultadoCicloEscola,
                                                 IRepositorioResultadoCicloTurma repositorioResultadoCicloTurma)
                                                 IRepositorioResultadoCicloDre repositorioResultadoCicloDre)
        {
            this.repositorioResultadoSme = repositorioResultadoSme ?? throw new ArgumentNullException(nameof(repositorioResultadoSme));
            this.repositorioResultadoDre = repositorioResultadoDre ?? throw new ArgumentNullException(nameof(repositorioResultadoDre));
            this.repositorioResultadoEscola = repositorioResultadoEscola ?? throw new ArgumentNullException(nameof(repositorioResultadoEscola));
            this.repositorioResultadoTurma = repositorioResultadoTurma ?? throw new ArgumentNullException(nameof(repositorioResultadoTurma));
            this.repositorioResultadoAluno = repositorioResultadoAluno ?? throw new ArgumentNullException(nameof(repositorioResultadoAluno));
            this.repositorioParticipacaoTurma = repositorioParticipacaoTurma ?? throw new ArgumentNullException(nameof(repositorioParticipacaoTurma));
            this.repositorioParticipacaoTurmaAreaConhecimento = repositorioParticipacaoTurmaAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoTurmaAreaConhecimento));
            this.repositorioParticipacaoUe = repositorioParticipacaoUe ?? throw new ArgumentNullException(nameof(repositorioParticipacaoUe));
            this.repositorioParticipacaoUeAreaConhecimento = repositorioParticipacaoUeAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoUeAreaConhecimento));
            this.repositorioParticipacaoDre = repositorioParticipacaoDre ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDre));
            this.repositorioParticipacaoDreAreaConhecimento = repositorioParticipacaoDreAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoDreAreaConhecimento));
            this.repositorioParticipacaoSme = repositorioParticipacaoSme ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSme));
            this.repositorioParticipacaoSmeAreaConhecimento = repositorioParticipacaoSmeAreaConhecimento ?? throw new ArgumentNullException(nameof(repositorioParticipacaoSmeAreaConhecimento));
            this.repositorioResultadoCicloSme = repositorioResultadoCicloSme ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloSme));
            this.repositorioResultadoCicloEscola = repositorioResultadoCicloEscola ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloEscola));
            this.repositorioResultadoCicloTurma = repositorioResultadoCicloTurma ?? throw new ArgumentNullException(nameof(repositorioResultadoCicloTurma));
            this.repositorioResultadoCicloDre = repositorioResultadoCicloDre ?? throw new System.ArgumentNullException(nameof(repositorioResultadoCicloDre));
        }

        public async Task<bool> Handle(AlterarResultadoPspCommand request, CancellationToken cancellationToken)
        {
            objResultado = request.Resultado;

            return request.Resultado.TipoResultado switch
            {
                TipoResultadoPsp.ResultadoAluno => await AlterarResultadoAluno(),
                TipoResultadoPsp.ResultadoSme => await AlterarResultadoSme(),
                TipoResultadoPsp.ResultadoDre => await AlterarResultadoDre(),
                TipoResultadoPsp.ResultadoEscola => await AlterarResultadoEscola(),
                TipoResultadoPsp.ResultadoTurma => await AlterarResultadoTurma(),
                TipoResultadoPsp.ResultadoParticipacaoTurma => await AlterarParticipacaoTurma(),
                TipoResultadoPsp.ParticipacaoTurmaAreaConhecimento => await AlterarParticipacaoTurmaAreaConhecimento(),
                TipoResultadoPsp.ResultadoParticipacaoUe => await AlterarParticipacaoUe(),
                TipoResultadoPsp.ParticipacaoUeAreaConhecimento => await AlterarParticipacaoUeAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoDre => await AlterarParticipacaoDre(),
                TipoResultadoPsp.ParticipacaoDreAreaConhecimento => await AlterarParticipacaoDreAreaConhecimento(),
                TipoResultadoPsp.ParticipacaoSme => await AlterarParticipacaoSme(),
                TipoResultadoPsp.ParticipacaoSmeAreaConhecimento => await AlterarParticipacaoSmeAreaConhecimento(),
                TipoResultadoPsp.ResultadoCicloSme => await AlterarResultadoCicloSme(),
                TipoResultadoPsp.ResultadoCicloEscola => await AlterarResultadoCicloEscola(),
                TipoResultadoPsp.ResultadoCicloTurma => await AlterarResultadoCicloTurma(),
                TipoResultadoPsp.ResultadoCicloDre => await AlterarResultadoCicloDre();
                   
        _ => false
            };
        }

        private async Task<bool> AlterarResultadoAluno()
        {
            var resultado = (ResultadoAluno)objResultado.Resultado;
            var result = await repositorioResultadoAluno.AlterarAsync(resultado);
            return result > 0;
        }
        
        private async Task<bool> AlterarResultadoTurma()
        {
            var resultado = (ResultadoTurma)objResultado.Resultado;
            var result = await repositorioResultadoTurma.AlterarAsync(resultado);
            return result > 0;
        }
        
        private async Task<bool> AlterarResultadoEscola()
        {
            var resultado = (ResultadoEscola)objResultado.Resultado;
            var result = await repositorioResultadoEscola.AlterarAsync(resultado);
            return result > 0;
        }
        
        private async Task<bool> AlterarResultadoDre()
        {
            var resultado = (ResultadoDre)objResultado.Resultado;
            var result = await repositorioResultadoDre.AlterarAsync(resultado);
            return result > 0;
        }

        private async Task<bool> AlterarResultadoSme()
        {
            var resultado = (ResultadoSme)objResultado.Resultado;
            var result = await repositorioResultadoSme.AlterarAsync(resultado);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurma()
        {
            var participacao = (ParticipacaoTurma)objResultado.Resultado;
            var result = await repositorioParticipacaoTurma.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoTurmaAreaConhecimento()
        {
            var participacao = (ParticipacaoTurmaAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoTurmaAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUe()
        {
            var participacao = (ParticipacaoUe)objResultado.Resultado;
            var result = await repositorioParticipacaoUe.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoUeAreaConhecimento()
        {
            var participacao = (ParticipacaoUeAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoUeAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoDre()
        {
            var participacao = (ParticipacaoDre)objResultado.Resultado;
            var result = await repositorioParticipacaoDre.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoDreAreaConhecimento()
        {
            var participacao = (ParticipacaoDreAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoDreAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoSme()
        {
            var participacao = (ParticipacaoSme)objResultado.Resultado;
            var result = await repositorioParticipacaoSme.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarParticipacaoSmeAreaConhecimento()
        {
            var participacao = (ParticipacaoSmeAreaConhecimento)objResultado.Resultado;
            var result = await repositorioParticipacaoSmeAreaConhecimento.AlterarAsync(participacao);
            return result > 0;
        }

        private async Task<bool> AlterarResultadoCicloSme()
        {
            var resultadoAlterar = (ResultadoCicloSme)objResultado.Resultado;
            var result = await repositorioResultadoCicloSme.AlterarAsync(resultadoAlterar);
            return result > 0;
        }
        
        private async Task<bool> AlterarResultadoCicloEscola()
        {
            var resultadoAlterar = (ResultadoCicloEscola)objResultado.Resultado;
            var result = await repositorioResultadoCicloEscola.AlterarAsync(resultadoAlterar);
            return result > 0;
        }
        
        private async Task<bool> AlterarResultadoCicloTurma()
        {
            var resultadoAlterar = (ResultadoCicloTurma)objResultado.Resultado;
            var result = await repositorioResultadoCicloTurma.AlterarAsync(resultadoAlterar);
            return result > 0;
        }


    private async Task<bool> AlterarResultadoCicloDre()
    {
        var participacao = (ResultadoCicloDre)ObjResultado.Resultado;
        var result = await repositorioResultadoCicloDre.AlterarAsync(participacao);
        return result > 0;
    }
}
}
