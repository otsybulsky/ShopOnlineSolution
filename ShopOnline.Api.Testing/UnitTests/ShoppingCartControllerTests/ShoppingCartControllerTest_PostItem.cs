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
        public async Task ShoppingCartControllerTest_PostItem_Ok()
        {
            var controller = await GetShoppingCartController();

            CartItemToAddDto cartItemToAddDto = new CartItemToAddDto { CartId = 1, ProductId = 2, Qty = 3 };

            CreatedAtActionResult result = (CreatedAtActionResult)(await controller.PostItem(cartItemToAddDto)).Result;

            CartItemDto data = (CartItemDto)result.Value;

            string actualResult = JsonSerializer.Serialize(data);
            string expectedResult = "{\"Id\":2,\"ProductId\":2,\"CartId\":1,\"ProductName\":\"Curology - Skin Care Kit\",\"ProductDescription\":\"A kit provided by Curology, containing skin care products\",\"ProductImageURL\":\"/Images/Beauty/Beauty2.png\",\"Price\":50.0,\"TotalPrice\":150.0,\"Qty\":3}";

            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_PostItem_NoContent()
        {
            var controller = await GetShoppingCartController();

            CartItemToAddDto cartItemToAddDto = new CartItemToAddDto { CartId = 1, ProductId = 1, Qty = 1 };

            NoContentResult result = (NoContentResult)(await controller.PostItem(cartItemToAddDto)).Result;            

            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_PostItem_AnyError()
        {
            var controller = await GetShoppingCartController(dropTable: true);
            CartItemToAddDto cartItemToAddDto = new CartItemToAddDto { CartId = 1, ProductId = 2, Qty = 1 };

            ObjectResult result = (ObjectResult)(await controller.PostItem(cartItemToAddDto)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "SQLite Error 1: 'no such table: CartItems'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
