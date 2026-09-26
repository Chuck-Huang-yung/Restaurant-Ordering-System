using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Helpers;
using ShoppingCart.Models;
using ShoppingCart.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Controllers
{
    public class SetMealController : Controller
    {
       

        // 套餐的資料 
        public static List<Menu> setMeals = new List<Menu>
        {
            new Menu {  Id = 1,Name = "A 套餐 - 個人獨推", Price = 250, Description = "泰式炒泡麵 + 香蕉煎餅 + 一杯泰奶", Img = "/images/images (1).jfif" },
            new Menu {  Id = 2,Name = "B 套餐 - 雙人套餐", Price = 700, Description = "任選三道菜 + 兩杯泰奶", Img = "/images/LINE_ALBUM_泰椰foodpanda_231003_1-600x439.jpg" },
            new Menu { Id = 3, Name = "C 套餐 - 家庭方案", Price = 1200, Description = "任選五道菜 + 冬蔭功海鮮湯 + 四杯泰奶", Img = "/images/Taipei-Thai-Food-Banner-e1609913742639.jpg" }
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
            return View(setMeals);
        }

        //顯示某一個商品的詳細資訊頁面（通常用於商品頁、單品頁、推薦商品點進來的頁面）
        public IActionResult Details(int id)
        {
            var meal = setMeals.FirstOrDefault(m => m.Id == id);
            if (meal == null) return NotFound();

            var cart = GetCart();
            ViewBag.CartQuantity = cart.Sum(c => c.Quantity);
            ViewBag.CartAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity);
            ViewBag.MenuOptions = MenuController.weeklyRecommended;
            return View(meal);
        }

        [HttpPost]
       
        //加入購物車
        public IActionResult AddToCart(int id, int quantity, string remark, [FromForm] string[] addons)
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
                            addonTotalPrice += price;
                    }
                }
            }

            var mealItem = setMeals.FirstOrDefault(m => m.Id == id);
            if (mealItem == null) return NotFound();

            var existingItem = cart.FirstOrDefault(c =>
               c.Name == mealItem.Name &&
                c.Remark == remark &&
                c.Addons.OrderBy(a => a).SequenceEqual(addonList.OrderBy(a => a))
            );

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
               
            }
            else
            {
                cart.Add(new Cart
                {
                    Id = mealItem.Id,
                    Name = mealItem.Name,
                    Quantity = quantity,
                    Addons = addonList,
                    AddonTotalPrice = 0,
                    Remark = remark,
                    BasePrice = mealItem.Price,
                    Price = mealItem.Price,

                    Img = mealItem.Img
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