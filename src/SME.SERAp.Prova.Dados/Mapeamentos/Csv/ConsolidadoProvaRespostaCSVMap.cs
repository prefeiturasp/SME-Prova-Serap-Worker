using CsvHelper.Configuration;
using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados
{
    public class ConsolidadoProvaRespostaCSVMap : ClassMap<ConsolidadoProvaRespostaDto>
    {
        public ConsolidadoProvaRespostaCSVMap()
        {
            Map(m => m.ProvaSerapId).Index(0).Name("prova_serap_id");
            Map(m => m.ProvaSerapEstudantesId).Index(1).Name("prova_serap_estudantes_id");
			Map(m => m.DreCodigoEol).Index(2).Name("dre_codigo_eol");
			Map(m => m.DreSigla).Index(3).Name("dre_sigla");
			Map(m => m.DreNome).Index(4).Name("dre_nome");
			Map(m => m.UeCodigoEol).Index(5).Name("ue_codigo_eol");
			Map(m => m.UeNome).Index(6).Name("ue_nome");
			Map(m => m.TurmaAnoEscolar).Index(7).Name("turma_ano_escolar");
			Map(m => m.TurmaAnoEscolarDescricao).Index(8).Name("turma_ano_escolar_descricao");
			Map(m => m.TurmaCodigo).Index(9).Name("turma_codigo");
			Map(m => m.TurmaDescricao).Index(10).Name("turma_descricao");
			Map(m => m.AlunoCodigoEol).Index(11).Name("aluno_codigo_eol");
		}
	}
}
