using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaAnoLegadoUseCase : ITratarProvaAnoLegadoUseCase
    {
        private readonly IServicoLog servicoLog;
        private readonly IMediator mediator;

        public TratarProvaAnoLegadoUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator;
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaLegadoId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);

                var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaLegadoId));
                var provaAtual = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(provaLegadoId));
                
                await mediator.Send(new ProvaRemoverAnosPorIdCommand(provaAtual.Id));

                if (provaAtual.Modalidade == Modalidade.EJA || provaAtual.Modalidade == Modalidade.CIEJA)
                {
                    var provaAnoDetalhes = (await mediator.Send(new ObterProvaAnoLegadoDetalhesPorIdQuery(provaLegadoId))).ToList();

                    var ids = provaAnoDetalhes.Select(t => t.TcpId).ToArray();
                    var provaAnosDepara = (await mediator.Send(new ObterProvaAnoDeparaPorTcpIdQuery(ids))).ToList();

                    var provaAnos = new List<ProvaAno>();
                    foreach (var provaAnoDetalhe in provaAnoDetalhes)
                    {
                        var provaAnoDepara = provaAnosDepara.FirstOrDefault(t => t.TcpId == provaAnoDetalhe.TcpId);
                        if (provaAnoDepara == null)
                            throw new Exception($"Tipo curriculo período {provaAnoDetalhe.TcpId} depara não configurado na tabela tipo_curriculo_periodo_ano");

                        if (!provaAnos.Any(t => t.Ano == provaAnoDepara.Ano && t.Modalidade == provaAnoDepara.Modalidade && t.EtapaEja == provaAnoDepara.EtapaEja))
                            provaAnos.Add(new ProvaAno(provaAnoDepara.Ano, provaAtual.Id, provaAnoDepara.Modalidade, provaAnoDepara.EtapaEja));
                    }

                    foreach (var provaAno in provaAnos)
                        await mediator.Send(new ProvaAnoIncluirCommand(provaAno));
                }
                else
                {
                    if (provaLegado == null)
                        throw new Exception($"Prova {provaLegadoId} não localizada!");

                    foreach (var ano in provaLegado.Anos)
                        await mediator.Send(new ProvaAnoIncluirCommand(new ProvaAno(ano, provaAtual.Id, provaAtual.Modalidade)));
                }

                if (provaAtual.AderirTodos && provaAtual.FormatoTai)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaPorProvaSync, provaAtual.Id));

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                throw new ErroException($"Erro ao tratar prova ano: {ex.Message}");
            }
        }
    }
}
