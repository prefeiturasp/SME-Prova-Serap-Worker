﻿using System;

namespace SME.SERAp.Prova.Infra
{
    public class AlunoEolDto : DtoBase
    {
        public long CodigoAluno { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeSocial { get; set; }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public int TipoTurno { get; set; }
        public long TurmaCodigo { get; set; }
        public int SituacaoAluno { get; set; }
        public long TurmaSerapId { get; set; }
        public DateTime DataSituacao { get; set; }

    }
}
