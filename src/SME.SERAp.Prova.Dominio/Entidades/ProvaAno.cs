using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAno : EntidadeBase
    {
        public ProvaAno(int ano, long provaId)
        {
            Ano = ano;
            ProvaId = provaId;
        }

        public int Ano { get; set; }
        public long ProvaId { get; set; }

    }
}
