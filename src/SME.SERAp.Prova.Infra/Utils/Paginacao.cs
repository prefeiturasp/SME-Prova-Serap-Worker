using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SERAp.Prova.Infra.Utils
{
    public class Paginacao
    {
        public class ListaPaginada<T> : List<T>
        {
            private int PaginaAtual { get; set; }
            public int TotalPaginas { get; private set; }
            public int QtdeItensPorPagina { get; private set; }
            public List<T> Lista { get; private set; }
            
            public ListaPaginada(List<T> lista, int qtdeItensPorPagina)
            {
                QtdeItensPorPagina = qtdeItensPorPagina;
                TotalPaginas = lista.Count();
                Lista = lista;
            }

            public List<List<T>> ObterTodasAsPaginas()
            {
                var paginasRetorno = new List<List<T>>();
                PaginaAtual = 1;
                int totalPaginas = (int)Math.Ceiling(TotalPaginas / (double)QtdeItensPorPagina);

                while (PaginaAtual <= totalPaginas)
                {
                    var pagina = ObterPagina(PaginaAtual);
                    paginasRetorno.Add(pagina);
                    PaginaAtual++;
                }

                return paginasRetorno;
            }

            public List<T> ObterPagina(int pagina)
            {
                return Lista.Skip((pagina - 1) * QtdeItensPorPagina).Take(QtdeItensPorPagina).ToList();
            }
        }
    }
}
