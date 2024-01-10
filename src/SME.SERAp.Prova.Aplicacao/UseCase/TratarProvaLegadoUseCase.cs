using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Exceptions;

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
            var provaLegadoId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
            if (provaLegadoId == 0)
                throw new NegocioException("O Id da prova deve ser informado.");

            var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaLegadoId));
            if (provaLegado == null)
                throw new Exception($"Prova {provaLegadoId} não localizada!");

            provaLegado.AderirTodos = await mediator.Send(new VerificaAderirTodosProvaLegadoQuery(provaLegadoId));

            var provaAtual = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(provaLegado.Id));

            if (provaLegado.Senha != null)
            {
                using var md5 = MD5.Create();
                md5.Initialize();
                var byteResult = md5.ComputeHash(Encoding.UTF8.GetBytes(provaLegado.Senha));
                provaLegado.Senha = string.Join("", byteResult.Select(x => x.ToString("x2")));
            }

            var modalidadeSerap = ObterModalidade(provaLegado.Modalidade, provaLegado.ModeloProva);
            var tipoProvaSerap = await ObterTipoProva(provaLegado.TipoProva);
            var provaParaTratar = ObterProvaTratar(provaLegado, modalidadeSerap, tipoProvaSerap);

            if (provaAtual == null)
            {
                provaAtual = new Dominio.Prova
                {
                    Id = await mediator.Send(new ProvaIncluirCommand(provaParaTratar))
                };
            }
            else
            {
                provaParaTratar.Id = provaAtual.Id;

                provaAtual.AderirTodos = provaParaTratar.AderirTodos;
                provaAtual.Multidisciplinar = provaParaTratar.Multidisciplinar;
                provaAtual.TipoProvaId = provaParaTratar.TipoProvaId;
                provaAtual.UltimaAtualizacao = provaParaTratar.UltimaAtualizacao;
                provaAtual.Disciplina = provaParaTratar.Disciplina;
                provaAtual.DisciplinaId = provaParaTratar.DisciplinaId;
                provaAtual.ProvaComProficiencia = provaParaTratar.ProvaComProficiencia;
                provaAtual.ApresentarResultados = provaParaTratar.ApresentarResultados;
                provaAtual.ApresentarResultadosPorItem = provaParaTratar.ApresentarResultadosPorItem;
                provaAtual.ExibirAudio = provaParaTratar.ExibirAudio;
                provaAtual.ExibirVideo = provaParaTratar.ExibirVideo;
                
                var verificaSePossuiRespostas = await mediator.Send(new VerificaProvaPossuiRespostasPorProvaIdQuery(provaAtual.Id));

                if (verificaSePossuiRespostas)
                {
                    provaAtual.InicioDownload = provaLegado.InicioDownload;
                    provaAtual.Inicio = provaLegado.Inicio;
                    provaAtual.Fim = provaLegado.Fim;
                    provaAtual.QtdItensSincronizacaoRespostas = provaLegado.QtdItensSincronizacaoRespostas;

                    await mediator.Send(new ProvaAtualizarCommand(provaAtual));
                    await mediator.Send(new RemoverProvasCacheCommand(provaAtual.Id));

                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAdesaoProva, new ProvaAdesaoDto(provaParaTratar.Id, provaParaTratar.LegadoId, provaParaTratar.AderirTodos)));
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaAnoTratar, provaLegado.Id));

                    return true;
                }

                await RemoverEntidadesFilhas(provaAtual);

                await mediator.Send(new ProvaAtualizarCommand(provaParaTratar));
            }

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAdesaoProva, new ProvaAdesaoDto(provaParaTratar.Id, provaParaTratar.LegadoId, provaParaTratar.AderirTodos)));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaAnoTratar, provaLegado.Id));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaGrupoPermissaoTratar, new ProvaIdsDto(provaParaTratar.Id, provaLegado.Id)));

            var contextosProva = (await mediator.Send(new ObterContextosProvaLegadoPorProvaIdQuery(provaLegadoId))).ToList();

            if (contextosProva.Any())
            {
                var ordem = 0;
                
                foreach (var contextoProvaDto in contextosProva.OrderBy(a => a.Id).ToList())
                {
                    var contextoProva = new ContextoProva(provaAtual.Id, ordem, contextoProvaDto.Titulo,
                        contextoProvaDto.ImagemCaminho, contextoProvaDto.Texto, contextoProvaDto.ImagemPosicao);
                    
                    await mediator.Send(new ContextoProvaIncluirCommand(contextoProva));
                    
                    ordem += 1;
                }
            }

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoSync, provaLegado.Id));
            await mediator.Send(new RemoverProvasCacheCommand(provaAtual.Id));

            return true;
        }

        private static Dominio.Prova ObterProvaTratar(ProvaLegadoDetalhesIdDto provaLegado, Modalidade modalidadeSerap, long tipoProvaSerap)
        {
            return new Dominio.Prova(0, provaLegado.Descricao, provaLegado.InicioDownload, provaLegado.Inicio, provaLegado.Fim,
                provaLegado.TotalItens, provaLegado.Id, provaLegado.TempoExecucao, provaLegado.Senha, provaLegado.PossuiBIB,
                provaLegado.TotalCadernos, modalidadeSerap, provaLegado.DisciplinaId, provaLegado.Disciplina, provaLegado.OcultarProva, provaLegado.AderirTodos,
                provaLegado.Multidisciplinar, (int)tipoProvaSerap, provaLegado.FormatoTai, provaLegado.QtdItensSincronizacaoRespostas, provaLegado.UltimaAtualizacao, 
                provaLegado.PermiteAvancarSemResponder, provaLegado.PermiteVoltarAoItemAnterior, provaLegado.ProvaComProficiencia, provaLegado.ApresentarResultados, provaLegado.ApresentarResultadosPorItem,
                provaLegado.ExibirAudio, provaLegado.ExibirVideo);
        }

        private static Modalidade ObterModalidade(ModalidadeSerap modalidade, ModeloProva modeloProva)
        {
            return modalidade switch
            {
                ModalidadeSerap.EnsinoFundamental => modeloProva == ModeloProva.EJA
                    ? Modalidade.EJA
                    : Modalidade.Fundamental,
                ModalidadeSerap.EducacaoInfantil => Modalidade.EducacaoInfantil,
                ModalidadeSerap.EnsinoMedio => Modalidade.Medio,
                ModalidadeSerap.EJAEnsinoFundamental => Modalidade.EJA,
                ModalidadeSerap.EJACIEJA => Modalidade.CIEJA,
                ModalidadeSerap.EJAEscolasEducacaoEspecial => Modalidade.EJA,
                _ => Modalidade.NaoCadastrado
            };
        }

        private async Task RemoverEntidadesFilhas(Dominio.Prova provaAtual)
        {
            await mediator.Send(new ProvaRemoverContextoProvaPorProvaIdCommand(provaAtual.Id));
            await mediator.Send(new RemoverQuestaoAlunoTaiPorProvaIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverCadernoAlunosPorProvaIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAnosPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAlternativasPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverQuestoesTriPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverQuestoesPorIdCommand(provaAtual.Id));
        }

        private async Task<long> ObterTipoProva(long tipoProvaLegadoId)
        {
            var tipoProva = await mediator.Send(new ObterTipoProvaPorLegadoIdQuery(tipoProvaLegadoId));
            var tipoProvaLegado = await mediator.Send(new ObterTipoProvaLegadoPorIdQuery(tipoProvaLegadoId));

            if (tipoProvaLegado is null || tipoProvaLegado?.LegadoId == 0)
                throw new Exception($"Tipo de prova {tipoProvaLegadoId} não localizado no legado.");

            if (tipoProva is null || tipoProva.Id == 0)
            {
                tipoProva = new TipoProva
                {
                    Id = await mediator.Send(new TipoProvaIncluirCommand(tipoProvaLegado))
                };
            }
            else
            {
                tipoProvaLegado.Id = tipoProva.Id;
                tipoProvaLegado.CriadoEm = tipoProva.CriadoEm;
                tipoProvaLegado.AtualizadoEm = tipoProva.AtualizadoEm;
                await mediator.Send(new TipoProvaAtualizarCommand(tipoProvaLegado));
            }

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarTipoProvaDeficiencia, tipoProva.Id));
            return tipoProva.Id;
        }
    }
}