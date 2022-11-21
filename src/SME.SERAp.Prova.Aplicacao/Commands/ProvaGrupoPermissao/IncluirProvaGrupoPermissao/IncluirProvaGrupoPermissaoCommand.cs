using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{ 
  public  class IncluirProvaGrupoPermissaoCommand : IRequest<bool>
    {
        public IncluirProvaGrupoPermissaoCommand(IEnumerable<ProvaGrupoPermissao> listaProvaGrupoPermissao)
        {
            ListaProvaGrupoPermissao = listaProvaGrupoPermissao;
        }

        public IEnumerable<ProvaGrupoPermissao> ListaProvaGrupoPermissao { get; set; }
    }
}