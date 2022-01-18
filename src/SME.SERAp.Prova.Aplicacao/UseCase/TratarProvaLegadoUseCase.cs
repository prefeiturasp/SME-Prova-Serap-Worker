using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaLegadoUseCase : ITratarProvaLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

                var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaId));

                if (provaLegado == null)
                    throw new Exception($"Prova {provaLegado} não localizada!");

                provaLegado.AderirTodos = await mediator.Send(new VerificaAderirTodosProvaLegadoQuery(provaId));

                var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(provaLegado.Id));

                if (provaLegado.Senha != null)
                {
                    using (var md5 = MD5.Create())
                    {
                        md5.Initialize();
                        var byteResult = md5.ComputeHash(Encoding.UTF8.GetBytes(provaLegado.Senha));
                        provaLegado.Senha = string.Join("", byteResult.Select(x => x.ToString("x2")));
                    }
                }

                var modalidadeSerap = ObterModalidade(provaLegado.Modalidade, provaLegado.ModeloProva);

                var provaParaTratar = new Dominio.Prova(0, provaLegado.Descricao, provaLegado.InicioDownload, provaLegado.Inicio, provaLegado.Fim,
                    provaLegado.TotalItens, provaLegado.Id, provaLegado.TempoExecucao, provaLegado.Senha, provaLegado.PossuiBIB,
                    provaLegado.TotalCadernos, modalidadeSerap, provaLegado.Disciplina, provaLegado.OcultarProva, provaLegado.AderirTodos, provaLegado.Multidisciplinar);

                if (provaAtual == null)
                {
                    provaAtual = new Dominio.Prova();
                    provaAtual.Id = await mediator.Send(new ProvaIncluirCommand(provaParaTratar));
                }
                else
                {
                    provaParaTratar.Id = provaAtual.Id;
                    provaAtual.AderirTodos = provaParaTratar.AderirTodos;
                    provaAtual.Multidisciplinar = provaParaTratar.Multidisciplinar;

                    var verificaSePossuiRespostas = await mediator.Send(new VerificaProvaPossuiRespostasPorProvaIdQuery(provaAtual.Id));
                    if (verificaSePossuiRespostas)
                    {
                        provaAtual.InicioDownload = provaLegado.InicioDownload;
                        provaAtual.Inicio = provaLegado.Inicio;
                        provaAtual.Fim = provaLegado.Fim;

                        await mediator.Send(new ProvaAtualizarCommand(provaAtual));
                        return true;
                    }

                    await RemoverEntidadesFilhas(provaAtual);
                    await mediator.Send(new ProvaAtualizarCommand(provaParaTratar));

                }

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAdesaoProva, new ProvaAdesaoDto(provaParaTratar.Id, provaParaTratar.LegadoId, provaParaTratar.AderirTodos)));

                foreach (var ano in provaLegado.Anos)
                {
                    await mediator.Send(new ProvaAnoIncluirCommand(new ProvaAno(ano, provaAtual.Id)));
                    if (provaLegado.PossuiBIB)
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBSync, new ProvaBIBSyncDto(provaAtual.Id, ano, provaAtual.TotalCadernos)));

                }

                var contextosProva = await mediator.Send(new ObterContextosProvaLegadoPorProvaIdQuery(provaId));

                if (contextosProva.Any())
                {
                    var ordem = 0;
                    foreach (var contextoProvaDto in contextosProva.OrderBy(a => a.Id).ToList())
                    {
                        var contextoProva = new ContextoProva(provaAtual.Id, ordem, contextoProvaDto.Titulo, contextoProvaDto.ImagemCaminho, contextoProvaDto.Texto, contextoProvaDto.ImagemPosicao);
                        await mediator.Send(new ContextoProvaIncluirCommand(contextoProva));
                        ordem += 1;
                    }
                }

                await mediator.Send(
                    new PublicaFilaRabbitCommand(RotasRabbit.QuestaoSync, provaLegado.Id));

                await mediator.Send(new RemoverProvasCacheCommand());
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
            return true;
        }

        private Modalidade ObterModalidade(ModalidadeSerap modalidade, ModeloProva modeloProva)
        {
            switch (modalidade)
            {
                case ModalidadeSerap.EnsinoFundamental:
                    if (modeloProva == ModeloProva.EJA)
                        return Modalidade.EJA;
                    else
                        return Modalidade.Fundamental;
                case ModalidadeSerap.EducacaoInfantil:
                    return Modalidade.EducacaoInfantil;
                case ModalidadeSerap.EnsinoMedio:
                    return Modalidade.Medio;
                case ModalidadeSerap.EJAEnsinoFundamental:
                    return Modalidade.EJA;
                case ModalidadeSerap.EJACIEJA:
                    return Modalidade.CIEJA;
                case ModalidadeSerap.EJAEscolasEducacaoEspecial:
                    return Modalidade.EJA;
                default:
                    return Modalidade.NaoCadastrado;
            }
        }

        private async Task RemoverEntidadesFilhas(Dominio.Prova provaAtual)
        {
            await mediator.Send(new ProvaRemoverContextoProvaPorProvaIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverCadernoAlunosPorProvaIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAnosPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAlternativasPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverQuestoesPorIdCommand(provaAtual.Id));
        }
    }
}