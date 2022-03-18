using System;

namespace SME.SERAp.Prova.Infra
{
    public class UsuarioCoreSsoDto
    {

        public Guid IdCoreSso { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public DateTime CriadoEm { get; set; }

        public UsuarioCoreSsoDto()
        {

        }
    }
}
