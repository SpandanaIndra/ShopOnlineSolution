using Microsoft.AspNetCore.Components;
using ShopOnlineBlazerWASM.Client.Services;
using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Client.Shared
{
    public partial class ProductCategoriesNavMenu:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; }
        public string ErrorMessage { get; set; }
        protected override async void OnInitialized()
        {
            try
            {

                ProductCategoryDtos=await ProductService.GetProductCategories();

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
    }
}
