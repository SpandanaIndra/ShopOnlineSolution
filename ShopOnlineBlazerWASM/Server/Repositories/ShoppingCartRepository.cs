using Microsoft.EntityFrameworkCore;
using ShopOnlineBlazerWASM.Server.Data;
using ShopOnlineBlazerWASM.Shared;
using ShopOnlineBlazerWASM.Shared.Dtos;

namespace ShopOnlineBlazerWASM.Server.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _context;
        public ShoppingCartRepository(ShopOnlineDbContext context)
        {

            _context = context;
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
           if(await CartItemExists(cartItemToAddDto.CartId,cartItemToAddDto.ProductId)==false)
            {
                var item = await (from product in _context.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = cartItemToAddDto.ProductId,
                                      Qty = cartItemToAddDto.Qty,
                                  }).SingleOrDefaultAsync();
                if(item != null)
                {
                    var result= await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _context.CartItems.AnyAsync(p=>p.CartId==cartId&& p.ProductId==productId);
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var cartitem = await _context.CartItems.FindAsync(id);

            if (cartitem != null)
            {
                 _context.CartItems.Remove(cartitem);
                await _context.SaveChangesAsync();
            }

            return cartitem;
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
          return await (from cart in _context.Carts join
                        cartItem in _context.CartItems on 
                        cart.Id equals cartItem.CartId
                        where cart.UserId==userId
                        select new CartItem
                        {
                            Id= cartItem.Id,
                            CartId = cartItem.CartId,
                            ProductId = cartItem.ProductId,
                            Qty = cartItem.Qty,
                        }).ToListAsync();
        }

        public async Task<CartItem> GetItem(int id)
        {
            var cartitem = await (from cart in _context.Carts
                                  join cartItem in _context.CartItems on cart.Id equals cartItem.CartId
                                  where cartItem.Id == id
                                  select new CartItem
                                  {
                                      Id = cartItem.Id,
                                      CartId = cartItem.CartId,
                                      ProductId = cartItem.ProductId,
                                      Qty = cartItem.Qty,
                                  }).SingleOrDefaultAsync();

            if (cartitem == null)
            {
                throw new InvalidOperationException($"CartItem with ID {id} not found.");
            }

            return cartitem;
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this._context.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await this._context.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
    }

