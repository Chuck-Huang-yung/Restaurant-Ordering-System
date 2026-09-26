using Microsoft.AspNetCore.Http;
using ShoppingCart.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace ShoppingCart.Helpers
{
    //用來將購物車 List<Cart> 存進 Session 或從 Session 取出，方便在整個網站中維持使用者購物車資料
    public static class SessionHelper
    {
        private const string CartSessionKey = "Cart";

        public static List<Cart> GetCart(HttpContext context)
        {
            var json = context.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json)) return new List<Cart>();
            return JsonSerializer.Deserialize<List<Cart>>(json) ?? new List<Cart>();
        }

        public static void SaveCart(HttpContext context, List<Cart> cart)
        {
            context.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }
    }
}
