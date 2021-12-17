﻿using System;

namespace SME.SERAp.Prova.Infra
{
    public class ConsolidadoProvaRespostaDto
    {
        public long ProvaSerapId { get; set; }
		public long ProvaSerapEstudantesId { get; set; }
		public string DreCodigoEol { get; set; }
		public string DreSigla { get; set; }
		public string DreNome { get; set; }
		public string UeCodigoEol { get; set; }
		public string UeNome { get; set; }
		public string TurmaAnoEscolar { get; set; }
		public string TurmaAnoEscolarDescricao { get; set; }
		public string TurmaCodigo { get; set; }
		public string TurmaDescricao { get; set; }
		public int ProvaQuantidadeQuestoes { get; set; }
		public long AlunoCodigoEol { get; set; }
		public string AlunoNome { get; set; }
		public string AlunoSexo { get; set; }
		public DateTime AlunoDataNascimento { get; set; }
		public string ProvaComponente { get; set; }
		public string ProvaCaderno { get; set; }
		public string AlunoFrequencia {get; set; }
		public long QuestaoId { get; set; }
		public int QuestaoOrdem { get; set; }
		public string Resposta { get; set; }


		//               vape.aluno_nome,
		//                  vape.aluno_sexo,
		//                  vape.aluno_data_nascimento,
		//                  vape.prova_componente,
		//                  vape.prova_caderno,
		//                  vape.aluno_frequencia, 
		//                  q.id as questao_id, 
		//                  q.ordem + 1 as questa_ordem,
		//                  case
		//                   when qar.alternativa_id<> null then a.numeracao
		//                   else qar.resposta
		//end as resposta

		public ConsolidadoProvaRespostaDto()
        {
        }
    }
}