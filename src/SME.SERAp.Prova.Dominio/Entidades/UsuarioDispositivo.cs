using System;

namespace SME.SERAp.Prova.Dominio
{
    public class UsuarioDispositivo : EntidadeBase
    {
        public UsuarioDispositivo()
        {

        }
        public UsuarioDispositivo(long ra, string dispositivoId, DateTime criadoEm, long? turmaId)
        {
            Ra = ra;
            DispositivoId = dispositivoId;
            CriadoEm = criadoEm;
            TurmaId = turmaId;
        }

        public long Ra { get; set; }
        public string DispositivoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public long? TurmaId { get; set; }

    }
}
