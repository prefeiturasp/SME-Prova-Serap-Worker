using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAno : EntidadeBase
    {
        public ProvaAno(string ano, long provaId)
        {
            Ano = ano;
            ProvaId = provaId;
        }

        public string Ano { get; set; }
        public long ProvaId { get; set; }

    }
}
