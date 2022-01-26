using System;

namespace SME.SERAp.Prova.Dominio
{
    public class TipoProvaDeficiencia : EntidadeBase
    {
        public TipoProvaDeficiencia()
        {

        }

        public TipoProvaDeficiencia(long deficienciaId, long tipoProvaId)
        {
            DeficienciaId = deficienciaId;
            TipoProvaId = tipoProvaId;
            CriadoEm = AtualizadoEm = DateTime.Now;
        }

        public long DeficienciaId { get; set; }
        public long TipoProvaId { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
