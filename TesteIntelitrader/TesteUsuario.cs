using Intelitrader.Controllers;
using Intelitrader.Models;
using Intelitrader.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TesteIntelitrader
{
    public class TesteUsuario
    {

        [Fact]
        public async Task getTeste_RegistroSaveGet_RetornaUsuarios()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            userMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var user = new Usuario() { firstName = "Marcel", age = 25};
            var Registros = new List<Usuario>();
            userMock.Setup(x => x.AdicionaUsuario(It.IsAny<Usuario>()))
                .Callback((Usuario parametro) =>  Registros.Add(parametro) );

            var resultadoEsperado = user;
            userMock.Setup(x => x.BuscaUsuario(It.IsAny<string>()))
                .ReturnsAsync((string parametro) => { return Registros.FirstOrDefault(x => x.Id == parametro); });

            //Act = Invocar metodo para testar
            var usuarioController = new UsuarioController(userMock.Object);
            await usuarioController.Post(user);
            var result = await usuarioController.GetUsuario(user.Id) as ObjectResult;


            // Assert = Verificar ação            
            Assert.Equal(result?.Value, resultadoEsperado);
            
        }




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
            var resultadoEsperado = StatusCodes.Status400BadRequest;
            userMock.Setup(x => x.AdicionaUsuario(It.IsAny<Usuario>()));// Usuario            
            //Act = Invocar metodo para testar
            var usuarioController = new UsuarioController(userMock.Object);
            var result = await usuarioController.Post(It.IsAny<Usuario>()) as ObjectResult;
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);

        }
        
        [Fact]
        public async Task postTeste_Ok_Salvar_Usuario()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = StatusCodes.Status200OK;
            var usuario = new Usuario();
            var usuarioController = new UsuarioController(userMock.Object);
            userMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            //Act = Invocar metodo para testar
            var result = await usuarioController.Post(usuario) as ObjectResult;
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);
        }

        [Fact]
        public async Task postTeste_SalvaUsuario_ErroSalvar()
        {
            // Arrange = Iniciar variáveis 
            var userMock = new Mock<IUsuarioRepository>();
            var resultadoEsperado = StatusCodes.Status400BadRequest;
            var usuario = new Usuario();
            var usuarioController = new UsuarioController(userMock.Object);
            userMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(false);

            //Act = Invocar metodo para testar
            var result = await usuarioController.Post(usuario) as ObjectResult;
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
            var result = await usuarioController.Put(Guid.NewGuid().ToString(), usuario) as ObjectResult;
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
            var result = await usuarioController.Delete(Guid.NewGuid().ToString()) as ObjectResult;
            // Assert = Verificar ação            
            Assert.Equal(resultadoEsperado, result?.StatusCode);

        }

        [Fact]
        public async Task GetUsuario_ID_invalido()
        {
            var userMock = new Mock<IUsuarioRepository>();
            var exception = await Assert.ThrowsAsync<Exception>(() => new UsuarioController(userMock.Object).GetUsuario(""));

            Assert.Equal("Id incorreto", exception.Message);
        }


        [Fact]
        public async Task put_ID_invalido()
        {
            var userMock = new Mock<IUsuarioRepository>();
            var usuario = new Usuario();
            var exception = await Assert.ThrowsAsync<Exception>(() => new UsuarioController(userMock.Object).Put("", usuario));

            Assert.Equal("Id incorreto", exception.Message);
        }

        [Fact]
        public async Task delete_ID_invalido()
        {
            var userMock = new Mock<IUsuarioRepository>();            
            var exception = await Assert.ThrowsAsync<Exception>(() => new UsuarioController(userMock.Object).Delete(""));

            Assert.Equal("Id incorreto", exception.Message);
        }

    }
}
