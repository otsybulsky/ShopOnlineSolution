using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        private static ProductDto BuildProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name
            };
        }
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            /*// use LINQ 
            return (from product in products
                    join productCategory in productCategories
                    on product.CategoryId equals productCategory.Id
                    select BuildProductDto(product, productCategory)).ToList();*/

            return (from product in products                    
                    select BuildProductDto(product)).ToList();
        }

        public static ProductDto ConvertToDto(this Product product)
        {            
            return BuildProductDto(product);
        }

        private static CartItemDto BuildCartItemDto(CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                CartId = cartItem.CartId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select BuildCartItemDto(cartItem, product)).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return BuildCartItemDto(cartItem, product);
        }

        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCSS = productCategory.IconCSS
                    }).ToList();
        }
    }
}
