﻿using System;

namespace SME.SERAp.Prova.Dominio
{
    public class CadernoAluno : EntidadeBase
    {
        public long AlunoId { get; set; }
        public long ProvaId { get; set; }
        public string Caderno { get; set; }
        public string CadernoTai { get; set; }

        public CadernoAluno(long alunoId, long provaId, string caderno, string cadernoTai = null)
        {
            AlunoId = alunoId;
            ProvaId = provaId;
            Caderno = caderno;
            CadernoTai = cadernoTai;
        }

        public CadernoAluno()
        {

        }
    }
}