using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ItemTaiDto : DtoBase
    {
        public ItemTaiDto()
        {

        }
        public ProvaFormatoTaiItem ProvaFormatoTaiItem;
        public int PermiteAvancarSemResponder;
        public int PermiteVoltarAoItemAnterior;
      
    }
}
