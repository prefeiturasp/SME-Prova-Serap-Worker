﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class AlunoQuestaoRespostasDto
    {
        public long QuestaoId { get; set; }
        public int QuestaoOrdem { get; set; }
        public string Resposta { get; set; }
    }
}
