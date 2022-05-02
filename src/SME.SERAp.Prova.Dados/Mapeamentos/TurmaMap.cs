﻿using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class TurmaMap : DommelEntityMap<Turma>
    {
        public TurmaMap()
        {
            ToTable("turma");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.TipoTurma).ToColumn("tipo_turma");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.ModalidadeCodigo).ToColumn("modalidade_codigo");
            Map(c => c.NomeTurma).ToColumn("nome");
            Map(c => c.TipoTurno).ToColumn("tipo_turno");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Semestre).ToColumn("semestre");
            Map(c => c.EtapaEja).ToColumn("etapa_eja");
            Map(c => c.SerieEnsino).ToColumn("serie_ensino");
        }
    }
}
