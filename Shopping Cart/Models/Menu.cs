
namespace ShoppingCart.Models
{
    public class Menu
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public bool Spicy { get; set; }
        public string? Img { get; set; }
    }
}
