using Intelitrader.Controllers;
using Intelitrader.Data.UsuarioContextImpl;
using Intelitrader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace TestingUser
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetAllBlogs_orders_by_name()
        {
            var data = new List<Usuario>
            {
                new Usuario { firstName = "BBB" },
                new Usuario { firstName = "ZZZ" },
                new Usuario { firstName = "AAA" },
            }.AsAsyncQueryable();

            var mockSet = new Mock<DbSet<Usuario>>();
            mockSet.As<IDbAsyncEnumerable<Usuario>>()
                .Setup(m => m.GetAsyncEnumerator());
            mockSet.As<IQueryable<Usuario>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);
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
public static class AsyncQueryable
{
    /// <summary>
    /// Returns the input typed as IQueryable that can be queried asynchronously
    /// </summary>
    /// <typeparam name="TEntity">The item type</typeparam>
    /// <param name="source">The input</param>
    public static IQueryable<TEntity> AsAsyncQueryable<TEntity>(this IEnumerable<TEntity> source)
        => new AsyncQueryable<TEntity>(source ?? throw new ArgumentNullException(nameof(source)));
}

public class AsyncQueryable<TEntity> : EnumerableQuery<TEntity>, IAsyncEnumerable<TEntity>, IQueryable<TEntity>
{
    public AsyncQueryable(IEnumerable<TEntity> enumerable) : base(enumerable) { }
    public AsyncQueryable(Expression expression) : base(expression) { }
    public IAsyncEnumerator<TEntity> GetEnumerator() => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());
    IQueryProvider IQueryable.Provider => new AsyncQueryProvider(this);

    class AsyncEnumerator : IAsyncEnumerator<TEntity>
    {
        private readonly IEnumerator<TEntity> inner;
        public AsyncEnumerator(IEnumerator<TEntity> inner) => this.inner = inner;
        public void Dispose() => inner.Dispose();
        public TEntity Current => inner.Current;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(inner.MoveNext());
#pragma warning disable CS1998 // Nothing to await
        public async ValueTask DisposeAsync() => inner.Dispose();
#pragma warning restore CS1998
    }

    class AsyncQueryProvider : IAsyncQueryProvider
    {
        private readonly IQueryProvider inner;
        internal AsyncQueryProvider(IQueryProvider inner) => this.inner = inner;
        public IQueryable CreateQuery(Expression expression) => new AsyncQueryable<TEntity>(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new AsyncQueryable<TElement>(expression);
        public object Execute(Expression expression) => inner.Execute(expression);
        public TResult Execute<TResult>(Expression expression) => inner.Execute<TResult>(expression);
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) => new AsyncQueryable<TResult>(expression);
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Execute<TResult>(expression);
    }
}