using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase: ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }

        public string TotalQuantity { get; set; }
        public string TotalPrice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            } 
        }

        protected async Task UpdateQty_Input(int itemId)
        {
            throw new NotImplementedException();
        }
        protected async Task UpdateQtyCartItem_Click(int itemId, int itemQty)
        {
            throw new NotImplementedException();
        }
        protected async Task DeleteCartItem_Click(int itemId)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(itemId);
            RemoveCartItem(itemId);

        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);

            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}
