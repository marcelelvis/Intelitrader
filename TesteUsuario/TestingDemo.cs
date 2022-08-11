using Intelitrader.Controllers;
using Intelitrader.Data.UsuarioContextImpl;
using Intelitrader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
//using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteUsuario
{
    [TestClass]
    internal class TestingDemo
    {
        [TestMethod]
        public async Task GetAllBlogs_orders_by_name()
        {
            var data = new List<Usuario>
            {
                new Usuario { firstName = "BBB" },
                new Usuario { firstName = "ZZZ" },
                new Usuario { firstName = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Usuario>>();
            mockSet.As<IDbAsyncEnumerable<Usuario>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Usuario>(data.GetEnumerator()));
            mockSet.As<IQueryable<Usuario>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Usuario>(data.Provider));
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<UsuarioContextImpl>();
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UsuarioController(mockContext.Object);
            var usersresponse = await service.GetUsuarios();
            var users = usersresponse.Value.ToList() ?? new List<Usuario>();
            Assert.AreEqual(3, users.Count);
            Assert.AreEqual("AAA", users[0].firstName);
            Assert.AreEqual("BBB", users[1].firstName);
            Assert.AreEqual("ZZZ", users[2].firstName);
        }
    }
}
