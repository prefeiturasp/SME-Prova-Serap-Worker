using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Dados.Mapeamentos
{
    public class ResultadoAlunoMap : DommelEntityMap<ResultadoAluno>
    {
        public ResultadoAlunoMap()
        {
            ToTable("ResultadoAluno");

            Map(c => c.Edicao).ToColumn("Edicao").IsKey();
            Map(c => c.AreaConhecimentoID).ToColumn("AreaConhecimentoID").IsKey();
            Map(c => c.uad_sigla).ToColumn("uad_sigla");
            Map(c => c.esc_codigo).ToColumn("esc_codigo");
            Map(c => c.AnoEscolar).ToColumn("AnoEscolar");
            Map(c => c.tur_codigo).ToColumn("tur_codigo");
            Map(c => c.tur_id).ToColumn("tur_id");
            Map(c => c.alu_matricula).ToColumn("alu_matricula").IsKey();
            Map(c => c.alu_nome).ToColumn("alu_nome");
       //     Map(c => c.ResultadoLegadoId).ToColumn("ResultadoLegadoID");
            Map(c => c.NivelProficienciaID).ToColumn("NivelProficienciaID");
            Map(c => c.Valor).ToColumn("Valor");
        }
    }
}

