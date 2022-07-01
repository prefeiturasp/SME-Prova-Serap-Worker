using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAlunoTaiSemCadernoDto
    {
        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
        public int Situacao { get; set; }
        public long ProvaLegadoId { get; set; }

        public bool Ativo()
        {
            var situacao = (SituacaoAluno)Situacao;
            return situacao == SituacaoAluno.Ativo
                || situacao == SituacaoAluno.PendenteRematricula
                || situacao == SituacaoAluno.Rematriculado
                || situacao == SituacaoAluno.SemContinuidade
                || situacao == SituacaoAluno.Concluido;
        }
    }
}
