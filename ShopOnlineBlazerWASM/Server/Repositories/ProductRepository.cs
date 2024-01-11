using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShopOnlineBlazerWASM.Server.Data;
using ShopOnlineBlazerWASM.Server.Repositories;
using ShopOnlineBlazerWASM.Shared;

namespace ShopOnlineBlazerWASM.Server.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;
        public ProductRepository(ShopOnlineDbContext context)
        {
           
            _context = context;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories=await _context.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category=await _context.ProductCategories.SingleOrDefaultAsync(i=>i.Id==id);
            return category;
        }

        public async Task<Product> GetItem(int id)
        {
           var product=await _context.Products.FindAsync(id);
            if(product != null)
            {
                return product;
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
          var products=  await _context.Products.ToListAsync();
            return products;
        }
    }
}
