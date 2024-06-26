﻿using System;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAnoDto : DtoBase
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public int TempoExecucao { get; set; }
        public DateTime Inclusao { get; set; }
        public int TotalItens { get; set; }
        public int TotalCadernos { get; set; }
        public long LegadoId { get; set; }
        public string Senha { get; set; }
        public bool PossuiBIB { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ano { get; set; }
        public int EtapaEja { get; set; }
        public int? QuantidadeRespostaSincronizacao { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public bool Deficiente { get; set; }
        public bool ProvaComProficiencia { get; set; }
        public bool ApresentarResultados { get; set; }
        public bool ApresentarResultadosPorItem { get; set; }
        public bool FormatoTai { get; set; }
        public bool FormatoTaiAvancarSemResponder { get; set; }
        public bool FormatoTaiVoltarItemAnterior { get; set; }
        public bool ExibirAudio { get; set; }
        public bool ExibirVideo { get; set; }

        public DateTime ObterDataInicioMais3Horas()
        {
            return Inicio.AddHours(3);
        }

        public DateTime ObterDataFimMais3Horas()
        {
            return Fim.AddHours(3);
        }

        public DateTime? ObterDataInicioDownloadMais3Horas()
        {
            return InicioDownload?.AddHours(3);
        }           
    }
}