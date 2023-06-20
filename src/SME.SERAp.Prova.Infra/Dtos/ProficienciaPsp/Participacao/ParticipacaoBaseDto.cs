﻿using CsvHelper.Configuration.Attributes;
using System;

namespace SME.SERAp.Prova.Infra
{
    public class ParticipacaoBaseDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }

        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }

        [Name("TotalPrevisto")]
        public int TotalPrevisto { get; set; }

        [Name("TotalPresente")]
        public int TotalPresente { get; set; }

        [Name("PercentualParticipacao")]
        public string _percentualParticipacao { get; set; }

        public decimal? PercentualParticipacao { get { return _percentualParticipacao.ConvertStringPraDecimalNullPsp(); } }

        public void ValidarCamposBase()
        {
            try
            {
                if (!ResultadoPsp.DecimalNullValido(this.PercentualParticipacao))
                    throw new Exception($"PercentualParticipacao: {this.PercentualParticipacao?.ToString()} inválido.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Dados inválidos -- {ex.Message} -- {ex.StackTrace}");
            }
        }
    }
}
