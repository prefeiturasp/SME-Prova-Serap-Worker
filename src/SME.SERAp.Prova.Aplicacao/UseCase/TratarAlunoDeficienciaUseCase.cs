using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoDeficienciaUseCase : ITratarAlunoDeficienciaUseCase
    {
        
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarAlunoDeficienciaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator;
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
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
                        servicoLog.Registrar(Dominio.Enums.LogNivel.Negocio, $"Tipo de deficiência do aluno não cadastrada no serap estudantes: AlunoRa:{alunoRa}, codigoDeficienciaEol:{deficiencia}");
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
