namespace Shopping_Cart.Models
{
    public class SetMeal
    {
        public string? Code { get; set; }  // A, B, C
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int? OriginalPrice { get; set; } //套餐價格固定
        public string? Img { get; set; }
        public int MaxChoices { get; set; }  // 用於任選限制
                                             //「最多可選幾個選項」的設定
    }
}
