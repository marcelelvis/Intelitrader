﻿using Intelitrader.Data;
using Intelitrader.Models;
using Microsoft.EntityFrameworkCore;



namespace Intelitrader.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioContext _context;
        public UsuarioRepository(UsuarioContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Usuario>> BuscaUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscaUsuario(string id)
        {
            return await _context.Usuarios
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public void AdicionaUsuario(Usuario usuario)
        {
            _context.Add(usuario);
        }


        public void AtualizaUsuario(Usuario usuario)
        {
            _context.Update(usuario);
        }      

        public void DeleteUsuario(Usuario usuario)
        {
            _context.Remove(usuario);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

       
    }
}
