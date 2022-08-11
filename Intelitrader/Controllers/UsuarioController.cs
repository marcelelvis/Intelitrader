using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Intelitrader.Data;
using Intelitrader.Models;
using Intelitrader.Repository;


namespace Intelitrader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            
            var usuarios = await _repository.BuscaUsuarios();

            return usuarios.Any()
               ? Ok(usuarios) : NotFound();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {           
            var usuario = await _repository.BuscaUsuario(id);

            return usuario != null
               ? Ok(usuario) : NotFound("Usuario não encontrado");
        }



        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            _repository.AdicionaUsuario(usuario);
            
            return await _repository.SaveChangesAsync()
                    ? Ok("Usuario adicionado")
                    : BadRequest("Erro ao salvar usuário");
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario)
        {
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuario não encontrado");

            usuarioBanco.firstName = usuario.firstName ?? usuarioBanco.firstName;
            usuarioBanco.surname = usuario.surname ?? usuarioBanco.surname;
            
            
            usuario.creationDate = usuario.creationDate != new DateTime()
                ? usuario.creationDate : usuarioBanco.creationDate;
            
            _repository.AtualizaUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync()
                ? Ok("Usuario atualizado")
                : BadRequest("Erro ao salvar usuario"); 
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) return NotFound("Usuario não encontrado");

            _repository.DeleteUsuario(usuarioBanco);

            return await _repository.SaveChangesAsync()
                ? Ok("Usuario deletado")
                : BadRequest("Erro ao deletar usuario");
        }
    }
}
