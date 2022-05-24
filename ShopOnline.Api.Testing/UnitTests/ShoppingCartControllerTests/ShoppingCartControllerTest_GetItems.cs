using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Api.Testing.UnitTests.ShoppingCartControllerTests
{
    public partial class ShoppingCartControllerTest
    {
        [Fact]
        public async Task ShoppingCartControllerTest_GetItems_All()
        {
            var controller = await GetShoppingCartController();

            OkObjectResult result = (OkObjectResult)(await controller.GetItems(1)).Result;

            IEnumerable<CartItemDto> data = (IEnumerable<CartItemDto>)result.Value;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, data.Count());
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItems_NoContent()
        {
            var controller = await GetShoppingCartController();

            NoContentResult result = (NoContentResult)(await controller.GetItems(2)).Result;            

            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItems_NoProductsExist()
        {
            var controller = await GetShoppingCartController();
            

            _shopOnlineDbContext.Products.RemoveRange(from p in _shopOnlineDbContext.Products select p);
            await _shopOnlineDbContext.SaveChangesAsync();

            ObjectResult result = (ObjectResult)(await controller.GetItems(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "No products exist in the system";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItems_AnyError()
        {
            var controller = await GetShoppingCartController(dropTable: true);            

            ObjectResult result = (ObjectResult)(await controller.GetItems(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "SQLite Error 1: 'no such table: Carts'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
