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
        public async Task ShoppingCartControllerTest_DeleteItem_Ok()
        {
            var controller = await GetShoppingCartController();

            OkObjectResult result = (OkObjectResult)(await controller.DeleteItem(1)).Result;
            CartItemDto data = (CartItemDto)result.Value;

            NoContentResult noContentResult = (NoContentResult)(await controller.GetItem(1)).Result;

            string actualResult = JsonSerializer.Serialize(data);
            string expectedResult = "{\"Id\":1,\"ProductId\":1,\"CartId\":1,\"ProductName\":\"Glossier - Beauty Kit\",\"ProductDescription\":\"A kit provided by Glossier, containing skin care, hair care and makeup products\",\"ProductImageURL\":\"/Images/Beauty/Beauty1.png\",\"Price\":100.0,\"TotalPrice\":100.0,\"Qty\":1}";

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_DeleteItem_ProductNotFound()
        {
            var controller = await GetShoppingCartController();

            _shopOnlineDbContext.Products.Remove(_shopOnlineDbContext.Products.Single(p => p.Id == 1));
            await _shopOnlineDbContext.SaveChangesAsync();

            ObjectResult result = (ObjectResult)(await controller.DeleteItem(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Product not found: 1";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_DeleteItem_AnyError()
        {
            var controller = await GetShoppingCartController(dropTable: true);

            ObjectResult result = (ObjectResult)(await controller.DeleteItem(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "An error occurred while saving the entity changes. See the inner exception for details.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
