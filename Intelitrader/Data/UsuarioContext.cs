﻿using Intelitrader.Models;
using Microsoft.EntityFrameworkCore;

namespace Intelitrader.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTime localDate = DateTime.Now;
            var usuario = modelBuilder.Entity<Usuario>();           
            usuario.Property(x => x.firstName).IsRequired();
            usuario.Property(x => x.surname).IsRequired(false);
            usuario.HasKey(x => x.Id);
            usuario.Property(x => x.Id).ValueGeneratedOnAdd();
            usuario.Property(x => x.age).IsRequired();
            usuario.Property(x => x.creationDate).HasDefaultValueSql("now()");
            
            
        }
    }
}
