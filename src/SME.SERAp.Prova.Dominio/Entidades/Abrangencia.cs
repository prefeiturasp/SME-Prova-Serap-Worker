namespace SME.SERAp.Prova.Dominio
{
    public class Abrangencia : EntidadeBase
    {

        public long UsuarioId { get; set; }
        public long GrupoId { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public long? TurmaId { get; set; }

        public Abrangencia()
        {

        }

        public Abrangencia(long usuarioId, long grupoId)
        {
            UsuarioId = usuarioId;
            GrupoId = grupoId;
        }

    }
}
