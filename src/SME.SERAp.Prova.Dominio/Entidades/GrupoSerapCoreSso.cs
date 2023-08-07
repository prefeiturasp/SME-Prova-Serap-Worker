using System;

namespace SME.SERAp.Prova.Dominio
{
    public class GrupoSerapCoreSso : EntidadeBase
    {

        public Guid IdCoreSso { get; set; }
        public string Nome { get; set; }        
        public DateTime CriadoEm { get; set; }

        public GrupoSerapCoreSso()
        {

        }

        public GrupoSerapCoreSso(Guid idCoreSso, string nome)
        {
            IdCoreSso = idCoreSso;
            Nome = nome;
            CriadoEm = DateTime.Now;
        }
    }
}
