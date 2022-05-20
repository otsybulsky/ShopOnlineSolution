using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Controllers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;
using Xunit.Abstractions;

namespace ShopOnline.Api.Testing.UnitTests.ProductControllerTests
{
    public partial class ProductControllerTest: ControllerTestBase
    {        
        public ProductControllerTest(ITestOutputHelper testOutput) : base(testOutput)
        {
            
        }

        private (IProductRepository, ShopOnlineDbContext) GetProductRepository()
        {
            var context = CreateContext();
            return (new ProductRepository(context), context);
        }

        private async Task ClearProducts(ShopOnlineDbContext context)
        {
            context.Products.RemoveRange(from product in context.Products select product);
            await context.SaveChangesAsync();
        }

        private async Task DropProducts(ShopOnlineDbContext context)
        {
            await context.Database.ExecuteSqlRawAsync("drop table Products");
        }

        private async Task<ProductController> GetProductController(bool emptyRepository = false, bool dropTable = false)
        {
            var (repo, context) = GetProductRepository();

            if (emptyRepository)
            {
                await ClearProducts(context);
            }

            if (dropTable)
            {
                await DropProducts(context);
            }

            return new ProductController(repo);
        }
    }
}
