﻿using Patrimonio.Contexts;
using Patrimonio.Domains;
using Patrimonio.Interfaces;
using Patrimonio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patrimonio.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly PatrimonioContext ctx;

        public UsuarioRepository(PatrimonioContext appContext)
        {
            ctx = appContext;
        }

        public Usuario Login(string email, string senha)
        {
            var usuario = ctx.Usuarios.FirstOrDefault(u => u.Email == email);
            var complt = ctx.Usuarios.FirstOrDefault(s => s.Senha == senha);

            if (complt.Senha.Length < 32 && complt.Senha[0] != '$')
            {
                complt.Senha = Criptografia.GerarHash(usuario.Senha);

                ctx.SaveChanges();

                if (usuario != null)
                {
                    bool confere = Criptografia.Comparar(senha, usuario.Senha);
                    if (confere)
                        return usuario;
                }
            }

            return ctx.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }
    }
}
