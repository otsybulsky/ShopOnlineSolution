using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Controllers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShopOnline.Api.Testing.UnitTests.ShoppingCartControllerTests
{
    public partial class ShoppingCartControllerTest : ControllerTestBase
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IProductRepository _productRepository;
        private ShopOnlineDbContext _shopOnlineDbContext;

        public ShoppingCartControllerTest(ITestOutputHelper testOutput) : base(testOutput)
        {
            
        }

        private void GetShoppingCartRepository()
        {
            _shopOnlineDbContext = CreateContext();
            _shoppingCartRepository = new ShoppingCartRepository(_shopOnlineDbContext);
            _productRepository = new ProductRepository(_shopOnlineDbContext);
        }        

        private async Task ClearShoppingCart(ShopOnlineDbContext ctx)
        {
            ctx.CartItems.RemoveRange(from c in ctx.CartItems select c);
            ctx.Carts.RemoveRange(from c in ctx.Carts select c);

            await ctx.SaveChangesAsync();
        }

        private async Task DropCarts(ShopOnlineDbContext ctx)
        {
            await ctx.Database.ExecuteSqlRawAsync("drop table CartItems");
            await ctx.Database.ExecuteSqlRawAsync("drop table Carts");
        }

        private async Task AddOneCart(ShoppingCartController controller)
        {
            CartItemToAddDto cartItemToAddDto = new CartItemToAddDto { CartId = 1, ProductId = 1, Qty = 1 };
            await controller.PostItem(cartItemToAddDto);
        }

        private async Task<ShoppingCartController> GetShoppingCartController(bool emptyRepository = false, bool dropTable = false)
        {
            GetShoppingCartRepository();

            var controller = new ShoppingCartController(_shoppingCartRepository, _productRepository);
            await AddOneCart(controller);

            if (emptyRepository)
            {
                await ClearShoppingCart(_shopOnlineDbContext);
            }

            if (dropTable)
            {
                await DropCarts(_shopOnlineDbContext);
            }

            return controller;
        }
    }
}
