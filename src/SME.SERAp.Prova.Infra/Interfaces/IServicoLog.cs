using SME.SERAp.Prova.Dominio.Enums;
using System;

namespace SME.SERAp.Prova.Infra.Interfaces
{
    public interface IServicoLog
    {
        void Registrar(Exception ex);
        void Registrar(string mensagem, Exception ex);
        void Registrar(LogNivel nivel, string erro, string observacoes, string stackTrace);
        void Registrar(LogNivel nivel, string mensagem);
    }
}
