using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await shopOnlineDbContext.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            bool cartExists = await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId);

            if(!cartExists)
            {
                var item = await (from product in shopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await shopOnlineDbContext.CartItems.AddAsync(item);
                    await shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {            
            var cartItem = await shopOnlineDbContext.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                shopOnlineDbContext.CartItems.Remove(cartItem);
                await shopOnlineDbContext.SaveChangesAsync();
            }

            return cartItem;            
        }

        public async Task<CartItem> GetItem(int id)
        {
            var item = await (from cart in shopOnlineDbContext.Carts
                          join cartItem in shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();

            return item;
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in shopOnlineDbContext.Carts
                          join cartItem in shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId= cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await shopOnlineDbContext.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
