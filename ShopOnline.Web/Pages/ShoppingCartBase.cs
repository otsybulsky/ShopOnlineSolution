using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase: ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }


        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }

        protected int TotalQuantity { get; set; }
        protected string TotalPrice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            } 
        }

        protected async Task UpdateQty_Input(int itemId)
        {
            await JSRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", itemId, true);
        }        
        protected async Task DeleteCartItem_Click(int itemId)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(itemId);
            RemoveCartItem(itemId);
            CartChanged();
        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private async void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);

            ShoppingCartItems.Remove(cartItemDto);

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                    
                    UpdateItemTotalPrice(returnedUpdateItemDto);
                    CartChanged();
                    await JSRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null) 
            { 
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }
        private void CalculateCartSummary()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
        }

        private void CartChanged()
        {
            CalculateCartSummary();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
