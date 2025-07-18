﻿using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Prova : EntidadeBase
    {
        public Prova()
        {
            Inclusao = DateTime.Now;
        }

        public Prova(long id, string descricao, DateTime? inicioDownload, DateTime inicio, DateTime fim, DateTime dataCorrecaoInicio, DateTime dataCorrecaoFim, int totalItens, long legadoId, int tempoExecucao, string senha, bool possuiBIB,
            int totalCadernos, Modalidade modalidade, long? disciplinaId, string disciplina, bool ocultarProva, bool aderirTodos, bool multidisciplinar, int tipoProvaId, bool formatoTai,
            int? qtdItensSincronizacaoRespostas, DateTime ultimaAtualizacao, ProvaFormatoTaiItem? provaFormatoTaiItem = Dominio.ProvaFormatoTaiItem.Todos, bool permiteAvancarSemResponderTai = false,
            bool permiteVoltarItemAnteriorTai = false, bool provaComProficiencia = false, bool apresentarResultados = false, bool apresentarResultadosPorItem = false, bool exibirAudio = false, bool exibirVideo = false, bool exibirNoBoletim = false)
        {
            Id = id;
            Descricao = descricao;
            InicioDownload = inicioDownload;
            Inicio = inicio;
            Fim = fim;
            DataCorrecaoInicio = dataCorrecaoInicio;
            DataCorrecaoFim = dataCorrecaoFim;
            TotalItens = totalItens;
            LegadoId = legadoId;
            Inclusao = DateTime.Now;
            TempoExecucao = tempoExecucao;
            Senha = senha;
            PossuiBIB = possuiBIB;
            TotalCadernos = totalCadernos;
            Modalidade = modalidade;
            DisciplinaId = disciplinaId;
            Disciplina = disciplina;
            OcultarProva = ocultarProva;
            AderirTodos = aderirTodos;
            Multidisciplinar = multidisciplinar;
            TipoProvaId = tipoProvaId;
            FormatoTai = formatoTai;
            ProvaFormatoTaiItem = provaFormatoTaiItem;
            QtdItensSincronizacaoRespostas = qtdItensSincronizacaoRespostas;
            UltimaAtualizacao = ultimaAtualizacao;
            PermiteAvancarSemResponderTai = permiteAvancarSemResponderTai;
            PermiteVoltarItemAnteriorTai = permiteVoltarItemAnteriorTai;
            ProvaComProficiencia = provaComProficiencia;
            ApresentarResultados = apresentarResultados;
            ApresentarResultadosPorItem = apresentarResultadosPorItem;
            ExibirAudio = exibirAudio;
            ExibirVideo = exibirVideo;
            ExibirNoBoletim = exibirNoBoletim;
        }

        public string Descricao { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime DataCorrecaoInicio { get; set; }
        public DateTime DataCorrecaoFim { get; set; }
        public DateTime Inclusao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
        public long? DisciplinaId { get; set; }
        public string Disciplina { get; set; }
        public long LegadoId { get; set; }
        public string Senha { get; set; }
        public bool PossuiBIB { get; set; }
        public int TotalCadernos { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool OcultarProva { get; set; }
        public bool AderirTodos { get; set; }
        public bool Multidisciplinar { get; set; }
        public int TipoProvaId { get; set; }
        public bool FormatoTai { get; set; }
        public ProvaFormatoTaiItem? ProvaFormatoTaiItem { get; set; }
        public int? QtdItensSincronizacaoRespostas { get; set; }
        public bool PermiteAvancarSemResponderTai { get; set; }
        public bool PermiteVoltarItemAnteriorTai { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public bool ProvaComProficiencia { get; set; }
        public bool ApresentarResultados { get; set; }
        public bool ApresentarResultadosPorItem { get; set; }
        public bool ExibirVideo { get; set; }
        public bool ExibirAudio { get; set; }
        public bool ExibirNoBoletim { get; set; }
    }
}
