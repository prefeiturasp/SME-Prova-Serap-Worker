using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarProvaGrupoPermissaoCommand : IRequest<bool>
    {
        public AlterarProvaGrupoPermissaoCommand(IEnumerable<ProvaGrupoPermissao> listaProvaGrupoPermissao)
        {
            ListaProvaGrupoPermissao = listaProvaGrupoPermissao;
        }
        public IEnumerable<ProvaGrupoPermissao> ListaProvaGrupoPermissao { get; set; }
    }
}