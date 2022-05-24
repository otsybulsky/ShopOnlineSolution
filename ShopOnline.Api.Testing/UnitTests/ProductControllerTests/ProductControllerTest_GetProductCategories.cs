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
        public async void ProductControllerTest_GetProductCategories_All()
        {
            var controller = await GetProductController();

            OkObjectResult result = (OkObjectResult)(await controller.GetProductCategories()).Result;

            IEnumerable<ProductCategoryDto> categories = (IEnumerable<ProductCategoryDto>)result.Value;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(4, categories.Count());
        }

        [Fact]
        public async void ProductControllerTest_GetProductCategories_AnyError()
        {
            var controller = await GetProductController(dropTable: true);

            ObjectResult result = (ObjectResult)(await controller.GetProductCategories()).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Error retrieving data from the database: SQLite Error 1: 'no such table: ProductCategories'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async void ProductControllerTest_GetItemsByCategory_Existed()
        {
            var controller = await GetProductController();

            int expectedCategoryId = 1;

            OkObjectResult result = (OkObjectResult)(await controller.GetItemsByCategory(expectedCategoryId)).Result;

            IEnumerable<ProductDto> products = (IEnumerable<ProductDto>)result.Value;

            var actualCategoryIds = (from p in products select p.CategoryId).ToList().Distinct();
            var actualCategoryId = actualCategoryIds.FirstOrDefault();

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, actualCategoryIds.Count());
            Assert.Equal(expectedCategoryId, actualCategoryId);            
        }

        [Fact]
        public async void ProductControllerTest_GetItemsByCategory_UnExisted()
        {
            var controller = await GetProductController();

            int expectedCategoryId = 1000;

            OkObjectResult result = (OkObjectResult)(await controller.GetItemsByCategory(expectedCategoryId)).Result;

            IEnumerable<ProductDto> products = (IEnumerable<ProductDto>)result.Value;
                        
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(0, products.Count());            
        }

        [Fact]
        public async void ProductControllerTest_GetItemsByCategory_AnyError()
        {
            var controller = await GetProductController(dropTable: true);

            ObjectResult result = (ObjectResult)(await controller.GetItemsByCategory(1)).Result;

            string actualResult = result.Value.ToString();
            string expectedResult = "Error retrieving data from the database: SQLite Error 1: 'no such table: Products'.";

            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
