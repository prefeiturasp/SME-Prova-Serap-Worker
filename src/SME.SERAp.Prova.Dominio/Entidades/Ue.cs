﻿using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Ue : EntidadeBase
    {
        public Ue()
        {
            DataAtualizacao = DateTime.Now;
        }
        public string CodigoUe { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public Dre Dre { get; set; }
        public long DreId { get; set; }        
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }        

        public void AdicionarDre(Dre dre)
        {
            if (dre != null)
            {
                Dre = dre;
                DreId = dre.Id;
            }
        }

        public bool EhUnidadeInfantil()
        {
            return TipoEscola == TipoEscola.EMEI || TipoEscola == TipoEscola.CEUEMEI
                || TipoEscola == TipoEscola.CEMEI || TipoEscola == TipoEscola.CECI
                || TipoEscola == TipoEscola.CEUCEMEI;
        }
    }
}