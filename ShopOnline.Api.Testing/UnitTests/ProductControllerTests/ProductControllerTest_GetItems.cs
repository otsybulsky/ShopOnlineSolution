using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Api.Testing.UnitTests.ProductControllerTests
{
    public partial class ProductControllerTest
    {
        [Fact]
        public async void ProductController_GetItems_All()
        {
            var controller = await GetProductController();

            OkObjectResult result = (OkObjectResult)(await controller.GetItems()).Result;

            IEnumerable<ProductDto> products = (IEnumerable<ProductDto>)result.Value;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(23, products.Count());
        }

        [Fact]
        public async void ProductController_GetItems_NotFound()
        {
            var controller = await GetProductController(emptyRepository: true);

            NotFoundResult result = (NotFoundResult)(await controller.GetItems()).Result;

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async void ProductController_GetItems_AnyError()
        {
            var controller = await GetProductController(dropTable: true);

            ObjectResult result = (ObjectResult)(await controller.GetItems()).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Error retrieving data from the database: SQLite Error 1: 'no such table: Products'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(actualResult, expectedResult);
        }
    }
}
