﻿using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAlunoReduzidaDto
    {
        public long ProvaAlunoId  { get; set; }
        public long AlunoRa { get; set; }
        public DateTime FinalizadoEm { get; set; }

        public ProvaAlunoReduzidaDto()
        {
        }
    }
}
