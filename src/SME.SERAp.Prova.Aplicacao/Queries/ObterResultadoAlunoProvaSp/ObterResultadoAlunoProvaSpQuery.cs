using MediatR;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterResultadoAlunoProvaSp
{
    public class ObterResultadoAlunoProvaSpQuery : IRequest<ResultadoAlunoProvaSpDto>
    {
        public int Edicao { get; set; }

        public int AreaDoConhecimento { get; set; }

        public string AlunoMatricula { get; set; }

        public ObterResultadoAlunoProvaSpQuery(int edicao, int areaDoConhecimento, string alunoMatricula)
        {
            Edicao = edicao;
            AreaDoConhecimento = areaDoConhecimento;
            AlunoMatricula = alunoMatricula;
        }
    }
}
