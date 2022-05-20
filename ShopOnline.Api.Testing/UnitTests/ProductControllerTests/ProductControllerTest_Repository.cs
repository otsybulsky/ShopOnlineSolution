using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Api.Testing.UnitTests.ProductControllerTests
{
    public partial class ProductControllerTest
    {
        [Fact]
        public async void GetAllUsersFromDatabase()
        {
            using var context = CreateContext();
            var users = await context.Users.ToListAsync();

            Assert.Equal(2, users.Count);

            Assert.Collection(users,
                u => Assert.Equal("Bob", u.UserName),
                u => Assert.Equal("Sarah", u.UserName));
        }

        [Fact]
        public async void GetAllProductsFromRepository()
        {
            var (productRepository, context) = GetProductRepository();
            
            var products = await productRepository.GetItems();

            //ObjectToOutput(products.Take(2).ToList());

            Assert.Equal(23, products.Count());
        }
    }
}
