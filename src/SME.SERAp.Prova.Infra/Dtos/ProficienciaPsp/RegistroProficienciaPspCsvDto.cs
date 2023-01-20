using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class RegistroProficienciaPspCsvDto
    {

        public RegistroProficienciaPspCsvDto()
        {

        }

        public long ProcessoId { get; set; }
        public object Registro { get; set; }

        public T ObterObjetoRegistro<T>() where T : class
        {
            return Registro?.ToString().ConverterObjectStringPraObjeto<T>();
        }

    }
}
