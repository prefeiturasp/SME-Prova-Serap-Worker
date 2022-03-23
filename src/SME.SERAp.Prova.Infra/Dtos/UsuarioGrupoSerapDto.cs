﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class UsuarioGrupoSerapDto
    {

        public long IdUsuarioSerap { get; set; }
        public long IdGrupoSerap { get; set; }

        public UsuarioGrupoSerapDto()
        {

        }

        public UsuarioGrupoSerapDto(long idUsuarioSerap, long idGrupoSerap)
        {
            IdUsuarioSerap = idUsuarioSerap;
            IdGrupoSerap = idGrupoSerap;
        }
    }
}
