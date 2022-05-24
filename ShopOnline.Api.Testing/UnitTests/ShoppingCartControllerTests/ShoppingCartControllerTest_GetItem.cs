using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopOnline.Api.Testing.UnitTests.ShoppingCartControllerTests
{
    public partial class ShoppingCartControllerTest
    {
        [Fact]
        public async Task ShoppingCartController_GetItem_ById()
        {
            var controller = await GetShoppingCartController();

            OkObjectResult result = (OkObjectResult)(await controller.GetItem(1)).Result;

            CartItemDto data = (CartItemDto)result.Value;

            string actualStr = JsonSerializer.Serialize(data);
            string expectedStr = "{\"Id\":1,\"ProductId\":1,\"CartId\":1,\"ProductName\":\"Glossier - Beauty Kit\",\"ProductDescription\":\"A kit provided by Glossier, containing skin care, hair care and makeup products\",\"ProductImageURL\":\"/Images/Beauty/Beauty1.png\",\"Price\":100.0,\"TotalPrice\":100.0,\"Qty\":1}";


            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedStr, actualStr);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItem_NoContent()
        {
            var controller = await GetShoppingCartController();

            NoContentResult result = (NoContentResult)(await controller.GetItem(2)).Result;

            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItem_NoProductExist()
        {
            var controller = await GetShoppingCartController();

            _shopOnlineDbContext.Products.RemoveRange(from p in _shopOnlineDbContext.Products where p.Id == 1 select p);
            await _shopOnlineDbContext.SaveChangesAsync();

            ObjectResult result = (ObjectResult)(await controller.GetItem(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Product not found: 1";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_GetItem_AnyError()
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
