using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarFrequenciaAlunoCommand : IRequest<bool>
    {
        public AtualizarFrequenciaAlunoCommand(long id, FrequenciaAluno frequencia)
        {
            Id = id;
            Frequencia = frequencia;
        }

        public long Id { get; set; }
        public FrequenciaAluno Frequencia { get; set; }
    }
}
