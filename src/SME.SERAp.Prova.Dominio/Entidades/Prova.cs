﻿using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Prova : EntidadeBase
    {
        public Prova()
        {
            Inclusao = DateTime.Now;
        }
        public Prova(long id, string descricao, DateTime? inicioDownload, DateTime inicio, DateTime fim, int totalItens, long legadoId, int tempoExecucao, string senha, bool possuiBIB, 
            int totalCadernos, Modalidade modalidade, string disciplina, bool ocultarProva, bool aderirTodos, bool multidisciplinar, int tipoProvaId, bool formatoTai, 
             int? qtdItensSincronizacaoRespostas, DateTime ultimaAtualizacao, ProvaFormatoTaiItem? provaFormatoTaiItem = null, int permiteAvancarSemResponderTai = 0, int permiteVoltarItemAnteriorTai = 0)
        {
            Id = id;
            Descricao = descricao;
            InicioDownload = inicioDownload;
            Inicio = inicio;
            Fim = fim;
            TotalItens = totalItens;
            LegadoId = legadoId;
            Inclusao = DateTime.Now;
            TempoExecucao = tempoExecucao;
            Senha = senha;
            PossuiBIB = possuiBIB;
            TotalCadernos = totalCadernos;
            Modalidade = modalidade;
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
        }

        public string Descricao { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inclusao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
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

        public int PermiteAvancarSemResponderTai { get; set; }
        public int PermiteVoltarItemAnteriorTai { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}
