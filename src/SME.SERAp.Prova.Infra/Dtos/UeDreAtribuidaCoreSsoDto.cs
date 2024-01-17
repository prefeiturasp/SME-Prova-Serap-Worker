using System;

namespace SME.SERAp.Prova.Infra
{
    public class UeDreAtribuidaCoreSsoDto : DtoBase
    {
        public string UeCodigo { get; set; }
        public Guid GrupoIdCoreSso { get; set; }
    }
}