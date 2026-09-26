using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using ShoppingCart.Helpers;

namespace ShoppingCart.Controllers
{
    // 讀取 Session 中的購物車
    // 將購物車存回 Session

    public class CartController : Controller
    {
        private List<Cart> GetCart() => SessionHelper.GetCart(HttpContext);
        private void SaveCart(List<Cart> cart) => SessionHelper.SaveCart(HttpContext, cart);

        //計算購物車數量
        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.TotalQuantity = cart.Sum(c => c.Quantity);
            ViewBag.TotalAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity);

            return View(cart);
        }


        //購物車刪除鍵
        //從購物車中移除特定商品

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        //發出請求
        //方法 只能處理 POST 請求 保護層

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)  //更新數量
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                item.Quantity = quantity > 0 ? quantity : 1; 
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {

            SaveCart(new List<Cart>()); // 清空購物車
            return Ok(new{});
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var cart = GetCart();

            // 從菜單資料來源取出商品資料
            var meal = SetMealController.setMeals.FirstOrDefault(m => m.Id == id);


            if (meal == null) return NotFound();

            var existingItem = cart.FirstOrDefault(c => c.Id == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new Cart
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Quantity = quantity,
                    BasePrice = meal.Price,
                    AddonTotalPrice = 0,
                    Addons = new List<string>(), 
                    Remark = "",
                    Img = meal.Img

                });
            }

            //使用者點擊「加入購物車」按鈕
            //計算回傳
            SaveCart(cart);

            return Json(new
            {
                success = true,
                message = "已加入購物車！",
                totalQuantity = cart.Sum(c => c.Quantity),
                totalAmount = cart.Sum(c => (c.BasePrice + c.AddonTotalPrice) * c.Quantity)
            });
        }
    }
}
