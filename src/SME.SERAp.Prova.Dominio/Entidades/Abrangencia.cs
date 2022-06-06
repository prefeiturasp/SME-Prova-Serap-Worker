using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Abrangencia : EntidadeBase
    {
        public Abrangencia()
        {

        }
        public Abrangencia(long usuarioId, long grupoId, long? dreId, long? ueId = null, long? turmaId = null, DateTime? inicio = null, DateTime? fim = null)
        {
            UsuarioId = usuarioId;
            GrupoId = grupoId;
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            Inicio = inicio;
            Fim = fim;
        }

        public long UsuarioId { get; set; }
        public long GrupoId { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public long? TurmaId { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
    }
}
