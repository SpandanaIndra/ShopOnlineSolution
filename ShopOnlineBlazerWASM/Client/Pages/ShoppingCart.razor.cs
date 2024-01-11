using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnlineBlazerWASM.Client.Services;
using ShopOnlineBlazerWASM.Shared;
using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Client.Pages
{
    public partial class ShoppingCart:ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CatrItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        public string TotalPrice { get; private set; }
        public int TotalQuantity { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(1);//userid 1

                CartChanged();


            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        // JavaScript function to display confirmation dialog
        [JSInvokable]
        public async Task ConfirmDelete(int itemId)
        {
            bool confirmed = await Js.InvokeAsync<bool>("confirm", "Click OK to remove this item from cart..??");
            if (confirmed)
            {
                await DeleteCartItem_Click(itemId);
            }
        }
        // Method to delete the item (you may have something similar)

        protected async Task DeleteCartItem_Click(int id)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.DeleteItem(id);
                RemoveCartItem(id);
                CartChanged();


            }
            catch (Exception)
            {

                //Log Exception
            }
        }
        private CatrItemDto GetCartItem(int id)
        {
          return ShoppingCartItems.FirstOrDefault(x => x.Id == id);

        }
        private void RemoveCartItem(int id)
        {
            var product=GetCartItem(id);
            if (product != null)
            {
                ShoppingCartItems.Remove(product);
            }
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

                    var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);
                   

               await UpdateItemTotalPrice(returnedUpdateItemDto);
                   /* CalculateCartSummaryTotals();
*/
                     CartChanged();

                     await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible",id, false);


                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);

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
       protected async Task UpdateQty_Input(int id)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible",id, true);
        }
        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }

        private async Task UpdateItemTotalPrice(CatrItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }

            //await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);

        }
        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);

        }
    }
}
