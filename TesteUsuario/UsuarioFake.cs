using Intelitrader.Controllers;
using Intelitrader.Data;
using Intelitrader.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteUsuario
{
    internal class UsuarioFactory
    {
        public UsuarioFactory() { }
        
        public static List<Usuario> getUsuarios() {
            var list = new List<Usuario>();
            list.Add(new Usuario());
            return list;
        }
    }
}
