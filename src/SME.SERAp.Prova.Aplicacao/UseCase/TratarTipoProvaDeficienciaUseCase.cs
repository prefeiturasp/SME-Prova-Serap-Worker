using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarTipoProvaDeficienciaUseCase : ITratarTipoProvaDeficienciaUseCase
    {
        private readonly IServicoLog servicoLog; 
        private readonly IMediator mediator;

        public TratarTipoProvaDeficienciaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var tipoProvaId = long.Parse(mensagemRabbit.Mensagem.ToString());
                if (tipoProvaId == 0)
                    throw new Exception("Tipo de prova não informado.");

                var tipoProva = await mediator.Send(new ObterTipoProvaPorIdQuery(tipoProvaId));
                if (tipoProva is null || tipoProva?.Id == 0)
                    throw new Exception($"Tipo de prova não localizado: {tipoProvaId}.");

                await mediator.Send(new RemoverTipoProvaDeficienciaPorTipoProvaIdCommand(tipoProvaId));

                if (tipoProva.ParaEstudanteComDeficiencia)
                {
                    var tiposDeficienciaLegado = await mediator.Send(new ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQuery(tipoProva.LegadoId));
                    foreach (Guid tipoDeficienciaId in tiposDeficienciaLegado)
                    {
                        var tipoDeficienciaSerap = await mediator.Send(new ObterTipoDeficienciaPorLegadoIdQuery(tipoDeficienciaId));
                        if (tipoDeficienciaSerap != null)
                        {
                            var tipoProvaDeficienciaIncluir = new TipoProvaDeficiencia(tipoDeficienciaSerap.Id, tipoProva.Id);
                            await mediator.Send(new TipoProvaDeficienciaIncluirCommand(tipoProvaDeficienciaIncluir));
                        }
                        else
                        {
                            servicoLog.Registrar(LogNivel.Informacao, $"Tipo de deficiência não cadastrada no serap estudantes: {tipoDeficienciaId}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
            return true;
        }
    }
}
