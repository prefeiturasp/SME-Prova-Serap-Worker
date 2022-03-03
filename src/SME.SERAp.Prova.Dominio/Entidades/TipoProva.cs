using System;

namespace SME.SERAp.Prova.Dominio
{
    public class TipoProva : EntidadeBase
    {
        public TipoProva()
        {

        }

        public TipoProva(long legadoId, string descricao, bool paraEstudanteComDeficiencia)
        {
            LegadoId = legadoId;
            Descricao = descricao;
            ParaEstudanteComDeficiencia = paraEstudanteComDeficiencia;
            CriadoEm = AtualizadoEm = DateTime.Now;
        }

        public long LegadoId { get; set; }

        public string Descricao { get; set; }

        public bool ParaEstudanteComDeficiencia { get; set; }

        public DateTime CriadoEm { get; set; }

        public DateTime AtualizadoEm { get; set; }
    }
}
