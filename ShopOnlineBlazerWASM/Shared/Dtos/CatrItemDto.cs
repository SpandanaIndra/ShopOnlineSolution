using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineBlazerWASM.Shared.Dtos
{
    public class CatrItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int Qty { get; set; }
    }
}
