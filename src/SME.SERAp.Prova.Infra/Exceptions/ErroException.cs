using System;
using System.Net;

namespace SME.SERAp.Prova.Infra.Exceptions
{
    public class ErroException : Exception
    {
        public ErroException(string mensagem, int statusCode = 500) : base(mensagem)
        {
            StatusCode = statusCode;
        }
        
        public ErroException(string mensagem, HttpStatusCode statusCode) : base(mensagem)
        {
            StatusCode = (int)statusCode;
        }        

        public int StatusCode { get; }
    }
}