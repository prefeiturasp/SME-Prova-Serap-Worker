using System;

namespace SME.SERAp.Prova.Infra.Interfaces
{
    public interface IServicoLog
    {
        void Registrar(Exception ex);

        void Registrar(string mensagem);

    }
}
