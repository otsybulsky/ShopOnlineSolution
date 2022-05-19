using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShopOnline.Api.Testing.UnitTests
{
    public class ProductControllerTest: IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ShopOnlineDbContext> _contextOptions;
        private readonly ITestOutputHelper log;

        public ProductControllerTest(ITestOutputHelper logger)
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<ShopOnlineDbContext>().UseSqlite(_connection).Options;

            // Create the schema and seed some data
            using var context = new ShopOnlineDbContext(_contextOptions);
            context.Database.EnsureCreated();
            this.log = logger;
        }

        ShopOnlineDbContext CreateContext() => new ShopOnlineDbContext(_contextOptions);

        public void Dispose()
        {
            _connection.Dispose();
        }

        private void LogObject(Object obj)
        {
            log.WriteLine(JsonSerializer.Serialize(obj));
        }

        [Fact]
        public async void GetAllUsersFromRepository()
        {
            using var context = CreateContext();
            var users = await context.Users.ToListAsync();

            Assert.Equal(2, users.Count);

            Assert.Collection(users,
                u => Assert.Equal("Bob", u.UserName),
                u => Assert.Equal("Sarah", u.UserName));
        }

        [Fact]
        public async void GetAllProductsFromRepository()
        {
            using var context = CreateContext();
            var products = await context.Products.ToListAsync();
                       
            LogObject(products.Take(2).ToList());

            Assert.Equal(23, products.Count);            
        }
    }
}
