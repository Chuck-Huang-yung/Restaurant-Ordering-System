using System.Collections.Generic;

namespace ShoppingCart.Models
{
    public class Cart
    {

        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty; //預設值為空字串，這樣即使沒填也不會造成錯誤
        public int Quantity { get; set; }
        public List<string> Addons { get; set; } = new();   //存放加購項目
        public int AddonTotalPrice { get; set; }
        public string? Remark { get; set; } //使用者備註
        public int BasePrice { get; set; }
        public int Price { get; set; }
        public string? Img { get; set; } = string.Empty; 

        
    }
}
