using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Dre : EntidadeBase
    {
        public Dre()
        {
            DataAtualizacao = DateTime.Now;
        }
        
        public string CodigoDre { get; set; }
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public virtual void AtualizarCampos(Dre dreAtualizada)
        {
            CodigoDre = dreAtualizada.CodigoDre;
            Nome = dreAtualizada.Nome;
            Abreviacao = dreAtualizada.Abreviacao;
            DataAtualizacao = DateTime.Now;
        }

        public bool DeveAtualizar(Dre dreQuePodeAlterar)
        {
            return Abreviacao != dreQuePodeAlterar.Abreviacao ||
                   CodigoDre != dreQuePodeAlterar.CodigoDre ||
                   Nome != dreQuePodeAlterar.Nome;
        }
    }
}
