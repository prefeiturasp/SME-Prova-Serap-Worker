
namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAno : EntidadeBase
    {
        public ProvaAno(string ano, long provaId, Modalidade modalidade, int etapaEja = 0)
        {
            Ano = ano;
            ProvaId = provaId;
            EtapaEja = etapaEja;
            Modalidade = modalidade;
        }

        public string Ano { get; set; }
        public long ProvaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int EtapaEja { get; set; }

    }
}
