using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineBlazerWASM.Shared.Dtos
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconCSS { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DOB { get; set; }
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
