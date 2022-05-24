using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopOnline.Api.Testing.UnitTests.ProductControllerTests
{
    public partial class ProductControllerTest
    {
        [Fact]
        public async void ProductController_GetItem_ById()
        {
            var controller = await GetProductController();

            OkObjectResult result = (OkObjectResult)(await controller.GetItem(2)).Result;

            ProductDto product = (ProductDto)result.Value;

            string actualProductStr = JsonSerializer.Serialize(product);
            string expectedProductStr = "{\"Id\":2,\"Name\":\"Curology - Skin Care Kit\",\"Description\":\"A kit provided by Curology, containing skin care products\",\"ImageURL\":\"/Images/Beauty/Beauty2.png\",\"Price\":50.0,\"Qty\":45,\"CategoryId\":1,\"CategoryName\":\"Beauty\"}";


            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedProductStr, actualProductStr);

        }

        [Fact]
        public async void ProductController_GetItem_NotFound()
        {
            var controller = await GetProductController(emptyRepository: true);

            BadRequestResult result = (BadRequestResult)(await controller.GetItem(2)).Result;

            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void ProductController_GetItem_AnyError()
        {
            var controller = await GetProductController(dropTable: true);

            ObjectResult result = (ObjectResult)(await controller.GetItem(2)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Error retrieving data from the database: SQLite Error 1: 'no such table: Products'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
