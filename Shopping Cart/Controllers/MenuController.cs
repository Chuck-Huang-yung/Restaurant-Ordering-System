using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Helpers;

namespace ShoppingCart.Controllers
{
    public class MenuController : Controller
    {
        //菜單數據
        public static List<Menu> weeklyRecommended = new List<Menu>
        {
        new Menu{ Name = "月亮蝦餅", Price = 200, Description = "蝦仁丁、魚漿，名氣 No.1", Spicy = false, Img = "/images/1_XEC0DxCVs4ikmuSqSeC1Ew.jpg" },
        new Menu { Name = "泰式冬蔭功海鮮湯", Price = 230, Description = "蝦子、蛤蠣、檸檬", Spicy = true, Img = "/images/0cbca4aaf2b7a4e5.jpg" },
        new Menu { Name = "椰子燉雞湯", Price = 300, Description = "香水椰子、土雞腿肉、枸杞", Spicy = false, Img = "/images/bk139-029.jpg" },
        new Menu { Name = "泰式辣味青木瓜沙拉", Price = 150, Description = "泰國辣椒、青木瓜絲、新鮮的大蒜、蝦米、魚露", Spicy = true, Img = "/images/COVER 3.1.jpg" },
        new Menu { Name = "打拋豬", Price = 180, Description = "辣炒豬絞肉、九層塔風味", Spicy = true, Img = "/images/Thai Basil Pork.jpg" },
        new Menu { Name = "泰式炒泡麵", Price = 160, Description = "酸辣泰式打拋豬拌炒泡麵、花枝", Spicy = true, Img = "/images/ff94de011d0ddb30.jpg" },
        new Menu { Name = "紅咖哩", Price = 200, Description = "肉類、紅咖哩醬和香滑的椰奶", Spicy = true, Img = "/images/76741ec4-324b-412e-950a-bc6c3d64987d.jpg" },
        new Menu { Name = "辛辣牛肉沙拉", Price = 290, Description = "舖有嫩牛肉條的清爽泰式沙拉", Spicy = true, Img = "/images/65d33298-6dfa-4864-9dcd-e83d8c18ceef.jpg" },
        new Menu { Name = "腰果炒雞肉", Price = 150, Description = "雞肉與腰果會和乾辣椒一起拌炒、各種蔬菜", Spicy = true, Img = "/images/2136cc48-4379-41ec-b02f-f29b89f360d0.jpg" },
        new Menu { Name = "清蒸檸檬魚", Price = 320, Description = "檸檬的酸香襯托了魚肉的細緻與甜味, 辣椒與蒜的香氣也讓這道菜加分不少!", Spicy = true, Img = "/images/fit.webp" },
        new Menu { Name = "綠咖哩雞", Price = 220, Description = "綠咖哩醬、椰奶、去骨雞腿肉", Spicy = false, Img = "/images/image-7-1024x682.webp" },
        new Menu { Name = "泰式椒麻雞", Price = 320, Description = "金黃酥脆、雞腿肉軟嫩多汁，搭配爽脆生菜和酸甜麻辣醬汁", Spicy = true, Img = "/images/fit (1).webp" },
        new Menu { Name = "泰式鮮蝦沙拉杯", Price = 170, Description = "蝦子、玉米粒、奇異果、番茄", Spicy = false, Img = "/images/fit (2).webp" }
        };

        // 讀取 Session 中的購物車
        // 將購物車存回 Session

        private List<Cart> GetCart() => SessionHelper.GetCart(HttpContext);
        private void SaveCart(List<Cart> cart) => SessionHelper.SaveCart(HttpContext, cart);

        //數量控制
        public IActionResult Index()
        {
          

            var cart = GetCart();
            ViewBag.CartQuantity = cart.Sum(c => c.Quantity);
            ViewBag.CartAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity);

            return View(weeklyRecommended);
        }

        //顯示某一個商品的詳細資訊頁面（通常用於商品頁、單品頁、推薦商品點進來的頁面）
        public IActionResult Details(string name)
        {
            var item = weeklyRecommended.FirstOrDefault(x => x.Name == name);
            if (item == null) return NotFound();

            var cart = GetCart();
            int cartQuantity = cart.Sum(c => c.Quantity);
            int cartAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity);

            ViewBag.CartQuantity = cartQuantity;
            ViewBag.CartAmount = cartAmount;

            return View(item);
        }


        [HttpPost]

        //加入購物車
        public IActionResult AddToCart(string name, int quantity, string remark, [FromForm] string[] addons)
        {
            var cart = GetCart();

            var addonList = new List<string>();
            int addonTotalPrice = 0;

            if (addons != null)
            {
                foreach (var addon in addons)
                {
                    var parts = addon.Split(':');
                    if (parts.Length == 2)
                    {
                        addonList.Add(parts[0]);
                        if (int.TryParse(parts[1], out int price))
                        {
                            addonTotalPrice += price;
                        }
                    }
                }
            }
                var menuItem = weeklyRecommended.FirstOrDefault(x => x.Name == name);
                if (menuItem == null)
                {
                    return Json(new { success = false, message = "找不到此餐點" });
                }

                var existingCartItem = cart.FirstOrDefault(c =>
                    c.Name == name &&
                    c.Remark == remark &&
                    c.Addons.SequenceEqual(addonList)
                );

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new Cart
                    {
                        Id = menuItem.Id,
                        Name = name,
                        Quantity = quantity,
                        Addons = addonList,
                        AddonTotalPrice = addonTotalPrice,
                        Remark = remark,
                        BasePrice = menuItem.Price,
                        Img = menuItem.Img
                    });
                }

                SaveCart(cart);


                return Json(new
                {
                    success = true,
                    message = "成功加入購物車！",
                    totalQuantity = cart.Sum(c => c.Quantity),
                    totalAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity)
                });
            }

        //用來顯示跳轉「購物車頁面」
        public IActionResult Cart()
        {
            var cart = GetCart();
            ViewBag.TotalQuantity = cart.Sum(c => c.Quantity);
            ViewBag.TotalAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity);
            return View(cart);
        }
    }
}