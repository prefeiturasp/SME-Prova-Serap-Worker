using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoDeficienciaUseCase : ITratarAlunoDeficienciaUseCase
    {
        
        private readonly IMediator mediator;

        public TratarAlunoDeficienciaUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoRa = long.Parse(mensagemRabbit.Mensagem.ToString());

                await mediator.Send(new RemoverAlunoDeficienciaPorAlunoRaCommand(alunoRa));

                var alunoDeficienciaEol = await mediator.Send(new ObterAlunoDeficienciaEolPorAlunoRaQuery(alunoRa));
                foreach (int deficiencia in alunoDeficienciaEol)
                {
                    var tipoDeficiencia = await mediator.Send(new ObterTipoDeficienciaPorCodigoEolQuery(deficiencia));
                    if (tipoDeficiencia != null)
                    {
                        var alunoDeficiencia = new AlunoDeficiencia(tipoDeficiencia.Id, alunoRa);
                        await mediator.Send(new AlunoDeficienciaIncluirCommand(alunoDeficiencia));
                    }
                    else
                    {
                        SentrySdk.CaptureMessage($"Tipo de deficiência do aluno não cadastrada no serap estudantes: AlunoRa:{alunoRa}, codigoDeficienciaEol:{deficiencia}", SentryLevel.Warning);
                    }
                }

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
            return true;
        }
    }
}
