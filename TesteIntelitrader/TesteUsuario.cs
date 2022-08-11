using Intelitrader.Controllers;
using Intelitrader.Models;
using Intelitrader.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TesteIntelitrader
{
    public class TesteUsuario
    {
        private readonly UsuarioRepository _UsuarioRepository;
        private readonly UsuarioController _usuarioController;
        private readonly IUsuarioRepository _IUsuarioRepository;
        
        
                

        [Fact]
        public async Task getTeste_buscarUsuario_RetornaUsuarios()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = new List<Usuario>() { new Usuario() };
            userMock.Setup(x => x.BuscaUsuarios()).ReturnsAsync(resultadoEsperado);        

            //Act = Invocar metodo para testar
            var usuarioController = new UsuarioController(userMock.Object);
            var result = await usuarioController.GetUsuarios() as ObjectResult;            
            
            // Assert = Verificar ação            
            Assert.Equal(result?.Value, resultadoEsperado);
            //userMock.Verify(x => x.BuscaUsuario(It.IsAny<int>()));
        }

        


        [Fact]
        public async Task postTeste_BadRequest_Salvar_Usuario()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = new string("Erro ao salvar usuário");
            userMock.Setup(x => x.AdicionaUsuario(It.IsAny<Usuario>()));// Usuario            
            //Act = Invocar metodo para testar
            var usuarioController = new UsuarioController(userMock.Object);
            var result = await usuarioController.Post(It.IsAny<Usuario>()) as ObjectResult;// Erro ao salvar usuario
            // Assert = Verificar ação            
            Assert.Equal(result?.Value, resultadoEsperado);

        }
        
        [Fact]
        public async Task postTeste_Ok_Salvar_Usuario()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = StatusCodes.Status200OK;
            var usuario = new Usuario();
            var usuarioController = new UsuarioController(userMock.Object);
            userMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            //Act = Invocar metodo para testar
            var result = await usuarioController.Post(usuario) as ObjectResult;// Erro ao salvar usuario
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);
        }

        
        [Fact]
        public async Task putTeste_AtualizaUsuario_ErroAtualizar()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = StatusCodes.Status404NotFound;
            var usuario = new Usuario();
            var usuarioController = new UsuarioController(userMock.Object);
            userMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            //Act = Invocar metodo para testar
            var result = await usuarioController.Put(1, usuario) as ObjectResult;// Erro ao salvar usuario
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);

        }
        
        [Fact]
        public async Task deleteTeste_buscarUsuario_RetornaUsuarios()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = StatusCodes.Status404NotFound;
            var usuario = new Usuario();
            var usuarioController = new UsuarioController(userMock.Object);
            userMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);
            //Act = Invocar metodo para testar
            var result = await usuarioController.Delete(1) as ObjectResult;// Erro ao salvar usuario
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);

        }
    }
}
