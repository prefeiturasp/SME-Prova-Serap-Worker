using System;

namespace SME.SERAp.Prova.Dominio
{ 
    public class ProvaAlunoReabertura : EntidadeBase
    {
        public ProvaAlunoReabertura()
        {
        }

        public long ProvaId { get; set; }
        public long AlunoRA { get; set; }
        public string LoginCoresso { get; set; }
        public Guid UsuarioCoresso { get; set; }
        public Guid GrupoCoresso { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AlteradoEm { get; set; }

    }
}
