using SME.SERAp.Prova.Dominio;
using System;
using Xunit;

namespace SME.SERAp.Dominio.Test.Entities
{
    public class ProvaTeste
    {
        [Fact]
        public void DeveAtribuirPropriedadesCorretamente()
        {
            string descricao = "Prova SAEB";
            DateTime? inicioDownload = new DateTime(2025, 6, 1, 8, 0, 0);
            DateTime inicio = new DateTime(2025, 6, 1, 9, 0, 0);
            DateTime fim = new DateTime(2025, 6, 1, 11, 0, 0);
            DateTime inclusao = new DateTime(2025, 5, 20);
            int totalItens = 45;
            int tempoExecucao = 90;
            long? disciplinaId = 101;
            string disciplina = "Matemática";
            long legadoId = 2001;
            string senha = "abc123";
            bool possuiBIB = true;
            int totalCadernos = 2;
            Modalidade modalidade = Modalidade.Fundamental;
            bool ocultarProva = false;
            bool aderirTodos = true;
            bool multidisciplinar = false;
            int tipoProvaId = 7;
            bool formatoTai = true;
            ProvaFormatoTaiItem? provaFormatoTaiItem = ProvaFormatoTaiItem.Todos;
            int? qtdItensSincronizacaoRespostas = 5;
            bool permiteAvancar = true;
            bool permiteVoltar = false;
            DateTime ultimaAtualizacao = DateTime.UtcNow;
            bool provaComProficiencia = true;
            bool apresentarResultados = true;
            bool apresentarResultadosPorItem = false;
            bool exibirVideo = true;
            bool exibirAudio = false;
            bool exibirNoBoletim = true;

            var prova = new SME.SERAp.Prova.Dominio.Prova
            {
                Descricao = descricao,
                InicioDownload = inicioDownload,
                Inicio = inicio,
                Fim = fim,
                Inclusao = inclusao,
                TotalItens = totalItens,
                TempoExecucao = tempoExecucao,
                DisciplinaId = disciplinaId,
                Disciplina = disciplina,
                LegadoId = legadoId,
                Senha = senha,
                PossuiBIB = possuiBIB,
                TotalCadernos = totalCadernos,
                Modalidade = modalidade,
                OcultarProva = ocultarProva,
                AderirTodos = aderirTodos,
                Multidisciplinar = multidisciplinar,
                TipoProvaId = tipoProvaId,
                FormatoTai = formatoTai,
                ProvaFormatoTaiItem = provaFormatoTaiItem,
                QtdItensSincronizacaoRespostas = qtdItensSincronizacaoRespostas,
                PermiteAvancarSemResponderTai = permiteAvancar,
                PermiteVoltarItemAnteriorTai = permiteVoltar,
                UltimaAtualizacao = ultimaAtualizacao,
                ProvaComProficiencia = provaComProficiencia,
                ApresentarResultados = apresentarResultados,
                ApresentarResultadosPorItem = apresentarResultadosPorItem,
                ExibirVideo = exibirVideo,
                ExibirAudio = exibirAudio,
                ExibirNoBoletim = exibirNoBoletim
            };

            Assert.Equal(descricao, prova.Descricao);
            Assert.Equal(inicioDownload, prova.InicioDownload);
            Assert.Equal(inicio, prova.Inicio);
            Assert.Equal(fim, prova.Fim);
            Assert.Equal(inclusao, prova.Inclusao);
            Assert.Equal(totalItens, prova.TotalItens);
            Assert.Equal(tempoExecucao, prova.TempoExecucao);
            Assert.Equal(disciplinaId, prova.DisciplinaId);
            Assert.Equal(disciplina, prova.Disciplina);
            Assert.Equal(legadoId, prova.LegadoId);
            Assert.Equal(senha, prova.Senha);
            Assert.Equal(possuiBIB, prova.PossuiBIB);
            Assert.Equal(totalCadernos, prova.TotalCadernos);
            Assert.Equal(modalidade, prova.Modalidade);
            Assert.Equal(ocultarProva, prova.OcultarProva);
            Assert.Equal(aderirTodos, prova.AderirTodos);
            Assert.Equal(multidisciplinar, prova.Multidisciplinar);
            Assert.Equal(tipoProvaId, prova.TipoProvaId);
            Assert.Equal(formatoTai, prova.FormatoTai);
            Assert.Equal(provaFormatoTaiItem, prova.ProvaFormatoTaiItem);
            Assert.Equal(qtdItensSincronizacaoRespostas, prova.QtdItensSincronizacaoRespostas);
            Assert.Equal(permiteAvancar, prova.PermiteAvancarSemResponderTai);
            Assert.Equal(permiteVoltar, prova.PermiteVoltarItemAnteriorTai);
            Assert.Equal(ultimaAtualizacao, prova.UltimaAtualizacao);
            Assert.Equal(provaComProficiencia, prova.ProvaComProficiencia);
            Assert.Equal(apresentarResultados, prova.ApresentarResultados);
            Assert.Equal(apresentarResultadosPorItem, prova.ApresentarResultadosPorItem);
            Assert.Equal(exibirVideo, prova.ExibirVideo);
            Assert.Equal(exibirAudio, prova.ExibirAudio);
            Assert.Equal(exibirNoBoletim, prova.ExibirNoBoletim);
        }
    }
}
