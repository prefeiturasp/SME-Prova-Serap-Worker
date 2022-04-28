using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class ContextoProvaLegadoDto : DtoBase
    {
        public ContextoProvaLegadoDto()
        {

        }
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string ImagemCaminho { get; set; }
        public Posicionamento ImagemPosicao { get; set; }
    }
}
