using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
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
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

            var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaId));

            if (provaLegado == null)
                throw new System.Exception($"Prova {provaLegado} não localizada!");

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

            var modalidadeSerap = ObterModalidade(provaLegado.Modalidade);

            var provaParaTratar = new Dominio.Prova(0, provaLegado.Descricao, provaLegado.InicioDownload, provaLegado.Inicio, provaLegado.Fim,
                provaLegado.TotalItens, provaLegado.Id, provaLegado.TempoExecucao, provaLegado.Senha, provaLegado.PossuiBIB,
                provaLegado.TotalCadernos, modalidadeSerap);

            if (provaAtual == null)
            {
                provaAtual = new Dominio.Prova();
                provaAtual.Id = await mediator.Send(new ProvaIncluirCommand(provaParaTratar));
            }
            else
            {
                provaParaTratar.Id = provaAtual.Id;

                var verificaSePossuiRespostas = await mediator.Send(new VerificaProvaPossuiRespostasPorProvaIdQuery(provaAtual.Id));
                if (verificaSePossuiRespostas)
                    throw new System.Exception($"A prova {provaAtual.Id} possui respostas cadastradas por isto não será atualizada.");

                await RemoverEntidadesFilhas(provaAtual);
                await mediator.Send(new ProvaAtualizarCommand(provaParaTratar));
            }

            foreach (var ano in provaLegado.Anos)
            {
                await mediator.Send(new ProvaAnoIncluirCommand(new Dominio.ProvaAno(ano, provaAtual.Id)));
                if (provaAtual.PossuiBIB)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBSync, new ProvaBIBSyncDto(provaAtual.Id, ano, provaAtual.TotalCadernos)));

            }

            await mediator.Send(
                new PublicaFilaRabbitCommand(RotasRabbit.QuestaoSync, provaLegado.Id));

            return true;
        }

        private Modalidade ObterModalidade(ModalidadeSerap modalidade)
        {
            switch (modalidade)
            {
                case ModalidadeSerap.EnsinoFundamental:
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
            await mediator.Send(new ProvaRemoverCadernoAlunosPorProvaId(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAnosPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAlternativasPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverQuestoesPorIdCommand(provaAtual.Id));
        }
    }
}