using System;

namespace SME.SERAp.Prova.Dominio
{
    public class UsuarioSerapCoreSso : EntidadeBase
    {

        public Guid IdCoreSso { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }        
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }

        public UsuarioSerapCoreSso()
        {

        }

        public UsuarioSerapCoreSso(Guid idCoreSso, string login, string nome)
        {
            IdCoreSso = idCoreSso;
            Login = login;
            Nome = nome;
            AtualizadoEm = CriadoEm = DateTime.Now;
        }

        public void AtualizarDataAtualizadoEm()
        {
            this.AtualizadoEm = DateTime.Now;
        }
    }
}
