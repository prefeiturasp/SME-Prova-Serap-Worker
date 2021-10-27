﻿using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioCadernoAluno : RepositorioBase<CadernoAluno>, IRepositorioCadernoAluno
    {
        public RepositorioCadernoAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
    }
}
