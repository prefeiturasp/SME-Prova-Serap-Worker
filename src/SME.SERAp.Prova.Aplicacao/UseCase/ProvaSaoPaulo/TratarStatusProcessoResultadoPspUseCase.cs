using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicaca;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarStatusProcessoResultadoPspUseCase : ITratarStatusProcessoResultadoPspUseCase
    {

        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;
        private readonly IModel model;
        private long idArquivoResultadoPsp = 0;

        public TratarStatusProcessoResultadoPspUseCase(IMediator mediator,
                                            IServicoLog servicoLog,
                                            IModel model)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                idArquivoResultadoPsp = long.Parse(mensagemRabbit.Mensagem.ToString());
                var arquivoResultadoPsp = await mediator.Send(new ObterTipoResultadoPspQuery(idArquivoResultadoPsp));
                if (arquivoResultadoPsp == null) return false;
                StatusImportacao statusProcesso = (StatusImportacao)arquivoResultadoPsp.State;

                if (statusProcesso == StatusImportacao.Erro || statusProcesso == StatusImportacao.Processado)
                    return false;

                TipoResultadoPsp tipoResultadoProcesso = (TipoResultadoPsp)arquivoResultadoPsp.CodigoTipoResultado;

                var qtd = model.MessageCount(ResultadoPsp.ObterFilaTratarPorTipoResultadoPsp(tipoResultadoProcesso));
                if (qtd == 0)
                {
                    await AtualizaStatusDoProcesso(StatusImportacao.Processado);
                    await mediator.Send(new FinalizarProcessosPorTipoCommand(tipoResultadoProcesso));
                }
                else
                {
                    await Task.Delay(2000);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarStatusProcessoResultado, idArquivoResultadoPsp));
                }

                return true;
            }
            catch (Exception ex)
            {
                await RegistrarErroEAtualizarStatusProcesso(GetType().Name, ex);
                return false;
            }
        }

        public async Task AtualizaStatusDoProcesso(StatusImportacao status)
        {
            await mediator.Send(new AtualizarStatusArquivoResultadoPspCommand(idArquivoResultadoPsp, status));
        }

        public async Task RegistrarErroEAtualizarStatusProcesso(string nomeUseCase, Exception ex)
        {
            servicoLog.Registrar($"Fila {nomeUseCase}, Erro ao tratar status do processo: {idArquivoResultadoPsp}", ex);
            if (idArquivoResultadoPsp > 0)
                await AtualizaStatusDoProcesso(StatusImportacao.Erro);
        }
    }
}
