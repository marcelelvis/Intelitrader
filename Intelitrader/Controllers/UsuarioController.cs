using Intelitrader.Models;
using Intelitrader.Repository;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

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

            Log.Information("Rastreio - Get iniciado");


            var usuarios = await _repository.BuscaUsuarios();

            if (usuarios.Any() != null)
            {
                Log.Information("Get retornando usuarios");
                return Ok(usuarios);
            } else{
                Log.Information("Erro ao retorna usuarios");
                return NotFound();
            }
            
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(string id)
        {
            Log.Information("Rastreio - Get iniciado");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Id incorreto");
            }


            var usuario = await _repository.BuscaUsuario(id);

            if (usuario != null)
            {
                Log.Information("Rastreio - Get retornando usuarios");
                return Ok(usuario);
            }
            else
            {
                Log.Information("Rastreio - Erro ao retorna usuarios");
                return NotFound();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            Log.Information("Rastreio - Post iniciado");
            _repository.AdicionaUsuario(usuario);
            bool result = await _repository.SaveChangesAsync();
            
            if (result == false)
            {
                Log.Information("Rastreio - Usuario nao adicionado");
                return BadRequest("Usuario não adicionado");
            }
            
            string dados = $"- Informações usuarios - {JsonSerializer.Serialize(usuario)}";
            Log.Information("Rastreio - Usuario adicionado " + dados);
            return Ok("Usuario adicionado - Id: " + usuario.Id);

           
                
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Usuario usuario)
        {
            Log.Information("Rastreio - Put iniciado");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Id incorreto");
            }

            
            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) {
                Log.Information("Rastreio - Usuario não encontrado");
                return NotFound("Usuario não encontrado"); 
            }

            usuarioBanco.firstName = usuario.firstName ?? usuarioBanco.firstName;
            usuarioBanco.surname = usuario.surname ?? usuarioBanco.surname;
            
            _repository.AtualizaUsuario(usuarioBanco);
            bool result = await _repository.SaveChangesAsync();

            if (result == false)
            {
                Log.Information("Rastreio - Usuario nao atualizado");
                return BadRequest("Usuario não atualizado");
            }
            string dados = $"Informações usuarios - {JsonSerializer.Serialize(usuario)}";
            Log.Information("Rastreio - Usuario atualizado - " + dados);
           
            return Ok("Usuario atualizado");

            

        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Log.Information("Rastreio - Delete iniciado");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Id incorreto");
            }


            var usuarioBanco = await _repository.BuscaUsuario(id);
            if (usuarioBanco == null) {
                Log.Information("Rastreio - erro ao buscar usuario");
                return NotFound("Usuario não encontrado"); 
            }

            
            _repository.DeleteUsuario(usuarioBanco);

            bool result = await _repository.SaveChangesAsync();

            if (result == false)
            {
                Log.Information("Rastreio - Usuario nao removido");
                return BadRequest("Usuario não removido");
            }
            string dados = $"Informações usuarios - {JsonSerializer.Serialize(usuarioBanco)}";
            Log.Information("Rastreio - Usuario removido - " + dados);
            return Ok("Usuario removido");
        }
    }
}
