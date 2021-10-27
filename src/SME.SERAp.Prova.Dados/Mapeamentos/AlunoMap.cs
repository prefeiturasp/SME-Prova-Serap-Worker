﻿using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class AlunoMap : DommelEntityMap<Dominio.Aluno>
    {
        public AlunoMap()
        {
            ToTable("aluno");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.RA).ToColumn("ra");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Situacao).ToColumn("situacao");
        }
    }
}
