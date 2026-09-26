# 🌶️ 泰式料理線上點餐與電子商務系統 
**(Thai Restaurant Online Ordering & E-Commerce System)**

![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-ORM-388E3C?logo=nuget&logoColor=white)

---

## 💡 專案簡介**
> 本專案為一款基於 C# ASP.NET Core MVC 架構開發的「泰式料理線上點餐平台」。系統涵蓋了完整的餐飲電子商務生命週期，從前端的菜單瀏覽、複雜的套餐選擇，到後端的購物車狀態管理、訂單建立與模擬金流付款流程，充分展現了強型別語言在企業級 Web 應用中的嚴謹性與商業邏輯處理能力。

---

## ✨ 核心業務模組 (Core Business Features)

本系統深度模擬了真實餐飲業的線上接單流程，實作以下核心模組：

* **🍜 彈性餐點與套餐模組 (Menu & Combos)：** 
  支援單點菜色與「泰式超值套餐」的組合邏輯，使用者可自由搭配附餐與飲品，系統會自動計算套餐差價與總額。
* **🛒 購物車狀態管理 (Shopping Cart Lifecycle)：** 
  運用 Session / 關聯式資料庫嚴密管理使用者的購物車狀態。支援餐點加入、數量動態增減、移除，以及即時的小計與總金額動態試算。
* **👤 購買人資訊與表單驗證 (Customer Info & Validation)：** 
  結帳前整合完整的收件人/購買人資訊填寫（包含姓名、聯絡電話、外送地址或外帶備註）。前端與後端皆實作嚴謹的資料驗證 (Data Annotations)，確保訂單資料完整性。
* **💳 訂單結算與模擬付款 (Checkout & Payment)：** 
  實作端到端 (End-to-End) 的結帳流程，包含最終訂單確認頁面、模擬金流串接付款機制，以及付款成功後的訂單編號生成與狀態更新。

---

## 🛠️ 技術架構與系統設計 (Tech Stack)

* **後端框架：** C# / ASP.NET Core MVC
* **視圖引擎：** Razor Pages (動態 HTML 渲染)
* **資料存取：** Entity Framework Core (ORM) / LINQ 查詢
* **前端整合：** HTML5, CSS3, Bootstrap (RWD 響應式網頁設計)
* **架構模式：** 嚴格遵守 MVC (Model-View-Controller) 設計模式，將商業邏輯、資料存取與 UI 介面完美解耦。

---

## 📂 專案核心目錄導覽 (Directory Structure)

```text
Restaurant-Ordering-System/
├── Shopping Cart.sln          # 專案方案檔
└── Shopping Cart/
    ├── Controllers/           # 控制器 (處理加入購物車、結帳、路由邏輯)
    ├── Models/                # 資料模型 (餐點實體、購物車物件、訂單資訊)
    ├── Views/                 # Razor 視圖 (前端畫面、套餐選擇表單、結帳頁面)
    ├── wwwroot/               # 靜態資源 (CSS, JS, 泰式料理餐點圖片)
    └── appsettings.json       # 系統與資料庫環境設定檔
