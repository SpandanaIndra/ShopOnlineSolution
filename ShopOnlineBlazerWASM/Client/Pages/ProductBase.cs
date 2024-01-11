using Microsoft.AspNetCore.Components;
using ShopOnlineBlazerWASM.Client.Services;
using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Client.Pages
{
    public class ProductBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
     
        public IEnumerable<ProductDto> Product { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Product = await ProductService.GetItems();
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Product
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }
        protected string GetCategoryName(IGrouping<int,ProductDto> groupedProductDtos) 
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }

    }
}
