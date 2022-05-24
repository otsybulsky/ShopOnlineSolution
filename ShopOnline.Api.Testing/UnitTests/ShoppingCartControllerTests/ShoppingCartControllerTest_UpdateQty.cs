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
        public async Task ShoppingCartControllerTest_UpdateQty_Ok()
        {
            var controller = await GetShoppingCartController();
                        
            CartItemQtyUpdateDto cartItemQtyUpdateDto = new CartItemQtyUpdateDto { CartItemId = 1, Qty = 2 };

            OkObjectResult result = (OkObjectResult)(await controller.UpdateQty(1, cartItemQtyUpdateDto)).Result;
            CartItemDto data = (CartItemDto)result.Value;                        

            string actualResult = JsonSerializer.Serialize(data);
            string expectedResult = "{\"Id\":1,\"ProductId\":1,\"CartId\":1,\"ProductName\":\"Glossier - Beauty Kit\",\"ProductDescription\":\"A kit provided by Glossier, containing skin care, hair care and makeup products\",\"ProductImageURL\":\"/Images/Beauty/Beauty1.png\",\"Price\":100.0,\"TotalPrice\":200.0,\"Qty\":2}";

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);            
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShoppingCartControllerTest_UpdateQty_NotFound()
        {
            var controller = await GetShoppingCartController();

            CartItemQtyUpdateDto cartItemQtyUpdateDto = new CartItemQtyUpdateDto { CartItemId = 1, Qty = 2 };

            NotFoundResult result = (NotFoundResult)(await controller.UpdateQty(2, cartItemQtyUpdateDto)).Result;            

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            
        }

        [Fact]
        public async Task ShoppingCartControllerTest_UpdateQty_AnyError()
        {
            var controller = await GetShoppingCartController(dropTable: true);

            CartItemQtyUpdateDto cartItemQtyUpdateDto = new CartItemQtyUpdateDto { CartItemId = 1, Qty = 2 };
                        
            ObjectResult result = (ObjectResult)(await controller.UpdateQty(1, cartItemQtyUpdateDto)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "An error occurred while saving the entity changes. See the inner exception for details.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);

        }
    }
}
