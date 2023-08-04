using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class AmostraProvaTaiDto
    {
        public AmostraProvaTaiDto()
        {
            ListaConfigItens = new List<ConfigAnoItensProvaTaiDto>();
        }

        public long ProvaLegadoId { get; set; }
        public int DisciplinaId { get; set; }
        public int MatrizId { get; set; }
        public int NumeroItensAmostra { get; set; }
        public bool AvancarSemResponder { get; set; }
        public bool VoltarAoItemAnterior { get; set; }
        public List<ConfigAnoItensProvaTaiDto> ListaConfigItens { get; set; }
    }
}
