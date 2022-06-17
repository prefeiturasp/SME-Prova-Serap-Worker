using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaAnoLegadoUseCase : ITratarProvaAnoLegadoUseCase
    {

        private readonly IMediator mediator;

        public TratarProvaAnoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

                var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaId));
                var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(provaId));

                if (provaAtual.Modalidade == Modalidade.EJA || provaAtual.Modalidade == Modalidade.CIEJA)
                {
                    var provaAnoDetalhes = await mediator.Send(new ObterProvaAnoLegadoDetalhesPorIdQuery(provaId));
                    var provaAnoInserir = TratarProvaAnoEjaCieja(provaAnoDetalhes, provaAtual);
                    if (provaAnoInserir.Any())
                    {
                        foreach (ProvaAno provaAno in provaAnoInserir)
                        {
                            await mediator.Send(new ProvaAnoIncluirCommand(provaAno));
                        }
                    }
                }
                else
                {
                    if (provaLegado == null)
                        throw new Exception($"Prova {provaLegado} não localizada!");

                    foreach (var ano in provaLegado.Anos)
                    {
                        await mediator.Send(new ProvaAnoIncluirCommand(new ProvaAno(ano, provaAtual.Id, provaAtual.Modalidade)));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        private List<ProvaAno> TratarProvaAnoEjaCieja(IEnumerable<ProvaAnoDetalheDto> provaAnoDetalhes, Dominio.Prova provaSerapEstudantes)
        {
            var provaAnoRetorno = new List<ProvaAno>();
            var provaSerapEstudantesId = provaSerapEstudantes.Id;
            var modalidadeProva = provaSerapEstudantes.Modalidade;

            //EJA MODULAR
            var ejaModular = provaAnoDetalhes.Where(p => p.CurId == (int)CursoSerap.EJA_Modular);
            if (ListaPossuiDados(ejaModular))
            {
                var anosEjaModular = ObterAnosListaDistinct(ejaModular);
                foreach (string ano in anosEjaModular)
                {
                    provaAnoRetorno.Add(new ProvaAno(ano, provaSerapEstudantesId, modalidadeProva));
                }
            }

            var provaAnoDetalhesEjaEFundamental = provaAnoDetalhes.Where(p => p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA && p.TneId == (int)TipoNivelEnsinoSerap.Ensino_Fundamental);

            //Alfabetização            
            var alfabetizacao = provaAnoDetalhesEjaEFundamental.Where(p => (p.CurId == (int)CursoSerap.EJA || p.CurId == (int)CursoSerap.EJA_2017)
                                                                        && (p.Ano == "1" || p.Ano == "2"));
            if (ListaPossuiDados(alfabetizacao))
            {
                var anosAlfabetizacao = ObterAnosListaDistinct(alfabetizacao);
                foreach (string ano in anosAlfabetizacao)
                {
                    provaAnoRetorno.Add(new ProvaAno("1", provaSerapEstudantesId, modalidadeProva, int.Parse(ano)));
                }
            }

            //Básica 1
            var basicaI = provaAnoDetalhes.Where(p => (p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA && p.TneId == (int)TipoNivelEnsinoSerap.Ensino_Fundamental && p.Ano == "3" && (p.CurCodigo == "3" || p.CurCodigo == "9"))
                                                    || p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA_Regular && p.TneId == (int)TipoNivelEnsinoSerap.Ensino_Fundamental && p.Ano == "5" && p.CurCodigo == "3"
                                                    && p.CurId != (int)CursoSerap.EJA_Modular);

            if (ListaPossuiDados(basicaI))
                provaAnoRetorno.Add(new ProvaAno("2", provaSerapEstudantesId, modalidadeProva, 1));


            //Básica 2            
            var basicaII = provaAnoDetalhes.Where(p => (p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA_Especial || p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA_Regular)
                                                    && p.TneId == (int)TipoNivelEnsinoSerap.Ensino_Fundamental
                                                    && p.Ano == "4"
                                                    && (p.CurCodigo == "3" || p.CurCodigo == "11"));
            if (ListaPossuiDados(basicaII))
                provaAnoRetorno.Add(new ProvaAno("2", provaSerapEstudantesId, modalidadeProva, 2));


            //Final 1
            var finalI = provaAnoDetalhesEjaEFundamental.Where(p => p.Ano == "7"
                                                                && (p.CurCodigo == "3" || p.CurCodigo == "9"));
            if (ListaPossuiDados(finalI))
                provaAnoRetorno.Add(new ProvaAno("4", provaSerapEstudantesId, modalidadeProva, 1));

            //Final 2
            var finalII = provaAnoDetalhes.Where(p => p.TmeId == (int)TipoModalidadeEnsinoSerap.EJA_Regular && p.TneId == (int)TipoNivelEnsinoSerap.Ensino_Fundamental
                                                   && p.Ano == "8"
                                                   && p.CurCodigo == "3");
            if (ListaPossuiDados(finalII))
                provaAnoRetorno.Add(new ProvaAno("4", provaSerapEstudantesId, modalidadeProva, 2));

            //Complementar 1
            var complementarI = provaAnoDetalhesEjaEFundamental.Where(p => p.Ano == "5"
                                                                        && (p.CurCodigo == "3" || p.CurCodigo == "9"));
            if (ListaPossuiDados(complementarI))
                provaAnoRetorno.Add(new ProvaAno("3", provaSerapEstudantesId, modalidadeProva, 1));

            //Complementar 2            
            var complementarII = provaAnoDetalhesEjaEFundamental.Where(p => p.Ano == "6"
                                                                        && (p.CurCodigo == "3" || p.CurCodigo == "9"));
            if (ListaPossuiDados(complementarII))
                provaAnoRetorno.Add(new ProvaAno("3", provaSerapEstudantesId, modalidadeProva, 2));

            //CIEJA
            var cieja = provaAnoDetalhesEjaEFundamental.Where(p => (p.Ano == "1" || p.Ano == "2" || p.Ano == "3" || p.Ano == "4")
                                                                && p.CurCodigo == "2");
            if (ListaPossuiDados(cieja))
            {
                var anosCieja = ObterAnosListaDistinct(cieja);
                foreach (string ano in anosCieja)
                {
                    provaAnoRetorno.Add(new ProvaAno(ano, provaSerapEstudantesId, Modalidade.CIEJA));
                }
            }

            return provaAnoRetorno;
        }

        private List<string> ObterAnosListaDistinct(IEnumerable<ProvaAnoDetalheDto> lista)
        {
            return lista.Select(e => e.Ano).Distinct().ToList();
        }

        private bool ListaPossuiDados(IEnumerable<ProvaAnoDetalheDto> lista)
        {
            return lista != null && lista.Any();
        }
    }
}
