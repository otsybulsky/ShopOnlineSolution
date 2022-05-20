using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using System.Data.Common;
using System.Text.Json;
using Xunit.Abstractions;

namespace ShopOnline.Api.Testing.UnitTests.ProductControllerTests
{
    public class ControllerTestBase : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ShopOnlineDbContext> _contextOptions;
        private readonly ITestOutputHelper _testOutput;

        public ControllerTestBase(ITestOutputHelper testOutput)
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<ShopOnlineDbContext>().UseSqlite(_connection).Options;

            // Create the schema and seed some data
            using var context1 = new ShopOnlineDbContext(_contextOptions);
            context1.Database.EnsureCreated();

            _testOutput = testOutput;
        }

        protected ShopOnlineDbContext CreateContext() => new ShopOnlineDbContext(_contextOptions);

        public void ObjectToOutput(object obj)
        {
            _testOutput.WriteLine(JsonSerializer.Serialize(obj));
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
