using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;

        private const string storageKey = "ShoppingOnline_CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await localStorageService.GetItemAsync<List<CartItemDto>>(storageKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await localStorageService.RemoveItemAsync(storageKey);
        }

        public async Task SaveCollection(List<CartItemDto> cartItems)
        {
            await localStorageService.SetItemAsync(storageKey, cartItems);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            var data = await shoppingCartService.GetItems(HardCoded.UserId);

            if (data != null)
            {
                await localStorageService.SetItemAsync(storageKey, data);
            }            
            
            return data;
        }
    }
}
