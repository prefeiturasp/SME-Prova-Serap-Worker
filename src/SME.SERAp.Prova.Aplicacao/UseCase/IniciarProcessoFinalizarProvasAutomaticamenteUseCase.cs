using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IniciarProcessoFinalizarProvasAutomaticamenteUseCase : IIniciarProcessoFinalizarProvasAutomaticamenteUseCase
    {
        private readonly IMediator mediator;
        public IniciarProcessoFinalizarProvasAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            int tempoExtra = await ObterParametroTempoExtra();
            var modalidades = EnumHelperService.MapEnumToDictionary<Modalidade>();
            foreach(int modalidade in modalidades.Keys.Where(m => m > 0))
            {
                var provas = await mediator.Send(new ObterProvasIniciadasPorModalidadeQuery(modalidade));
                if (provas == null || !provas.Any())
                    continue;

                var idsProvas = provas.Select(p => p.ProvaId).Distinct();
                foreach (long provaId in idsProvas)
                {
                    var provasParaFinalizar = provas.Where(p => p.ProvaId == provaId && p.PodeFinalizarProva(tempoExtra));
                    if (provasParaFinalizar == null || !provasParaFinalizar.Any())
                        continue;

                    var provaParaFinalizar = MapearParaDto(provaId, provasParaFinalizar);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.FinalizarProvaAutomaticamente, provaParaFinalizar));
                }
            }
            return true;
        }

        private async Task<int> ObterParametroTempoExtra()
        {
            var tiposParaBuscar = new int[] { (int)TipoParametroSistema.TempoExtraProva };
            var parametrosParaUtilizar = await mediator.Send(new ObterParametroSistemaPorTiposEAnoQuery(tiposParaBuscar, DateTime.Now.Year));
            var parametroTempoExtra = parametrosParaUtilizar.FirstOrDefault();
            int tempoExtra = 600;
            if (parametroTempoExtra != null)
                tempoExtra = int.Parse(parametroTempoExtra.Valor);
            return tempoExtra;
        }

        private ProvaParaAtualizarDto MapearParaDto(long provaId, IEnumerable<ProvaAlunoDto> provasParaFinalizar)
        {
            return new ProvaParaAtualizarDto()
            {
                ProvaId = provaId,
                FinalizadoEm = DateTime.Now,
                Status = (int)ProvaStatus.FINALIZADA_AUTOMATICAMENTE_JOB,
                IdsProvasAlunos = provasParaFinalizar.Select(pa => pa.ProvaAlunoId).ToArray()
            };
        }
    }
}
