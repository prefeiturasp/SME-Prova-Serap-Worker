using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirListaProvaAdesaoCommand : IRequest<bool>
    {
        public List<ProvaAdesao> ListaProvaAdesao { get; set; }

        public InserirListaProvaAdesaoCommand(List<ProvaAdesao> listaProvaAdesao)
        {
            ListaProvaAdesao = listaProvaAdesao;
        }
    }
}
