using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Client.Services
{
    public interface IShoppingCartService
    {
        Task<CatrItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<List<CatrItemDto>> GetItems(int userId);
        Task<CatrItemDto> DeleteItem(int id);
        Task<CatrItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}
