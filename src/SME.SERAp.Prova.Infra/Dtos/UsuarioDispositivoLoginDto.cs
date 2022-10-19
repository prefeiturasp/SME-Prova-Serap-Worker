using System;

namespace SME.SERAp.Prova.Infra
{
    public class UsuarioDispositivoLoginDto
    {
        public UsuarioDispositivoLoginDto()
        {

        }

        public long Ra { get; set; }
        public string DispositivoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public long? TurmaId { get; set; }
    }
}
