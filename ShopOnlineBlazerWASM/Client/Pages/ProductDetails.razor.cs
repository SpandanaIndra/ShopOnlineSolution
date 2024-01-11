using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ShopOnlineBlazerWASM.Client.Authentication;
using ShopOnlineBlazerWASM.Client.Services;
using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Client.Pages
{
    public partial class ProductDetails:ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authenticationState { get; set; }
        [Inject]
        public AuthenticationStateProvider authstateprovider { get; set; }
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IProductService _service { get; set; }
        [Inject]
        public IJSRuntime _JS { get; set; }
        [Inject]
        public IShoppingCartService _shopCartService { get; set; }
        [Inject]
        public NavigationManager _navManager { get; set; }
        public ProductDto Product { get; set; }
        [Parameter]
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var customStateProvider=(CustomAuthenticationStateProvider)authstateprovider;
                var token = await customStateProvider.GetToken();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                    Product = await _service.GetItem(Id);
                    var shoppingCartItems = await _shopCartService.GetItems(1);//1 is hardcoded userid here
                    var totalQty = shoppingCartItems.Sum(i => i.Qty);
                    _shopCartService.RaiseEventOnShoppingCartChanged(totalQty);
                }

             
            }
            catch (Exception ex)
            {

               ErrorMessage= ex.Message;
            }
        }
        [JSInvokable]
        public async Task ConfirmAddToCart()
        {
            bool confirmed = await _JS.InvokeAsync<bool>("confirm", "Are you sure, Do you want to add this item to the cart?");
            if (confirmed)
            {
                await AddToCart_Click(new CartItemToAddDto
                {
                    CartId = 1,
                    ProductId = Product.Id,
                    Qty = 1
                });
            }
        }
        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await _shopCartService.AddItem(cartItemToAddDto);
                _navManager.NavigateTo("/shoppingcart");

              /*  if (cartItemDto != null)
                {
                    ShoppingCartItems.Add(cartItemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }

               ;*/
            }
            catch (Exception)
            {

                //Log Exception
            }
        }

    }
}
