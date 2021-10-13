using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTamanhoArquivosQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterTamanhoArquivosQuery(IEnumerable<Arquivo> arquivos)
        {
            Arquivos = arquivos;
        }

        public IEnumerable<Arquivo> Arquivos { get; set; }
    }
}
