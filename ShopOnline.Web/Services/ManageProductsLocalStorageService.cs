using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IProductService productService;

        private const string storageKey = "ShoppingOnline_ProductCollection";

        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            this.localStorageService = localStorageService;
            this.productService = productService;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await localStorageService.GetItemAsync<IEnumerable<ProductDto>>(storageKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await localStorageService.RemoveItemAsync(storageKey);
        }

        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await productService.GetItems();

            if (productCollection != null)
            {
                await localStorageService.SetItemAsync(storageKey, productCollection);
            }

            return productCollection;
        }
    }
}
