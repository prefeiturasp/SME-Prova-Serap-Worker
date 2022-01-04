using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAdesao : RepositorioBase<ProvaAdesao>, IRepositorioProvaAdesao
    {
        public RepositorioProvaAdesao(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
    }
}
