using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace FitMatch_BackEnd.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime? OrderTime { get; set; }
        [DisplayName("訂購時間")]
        public DateTime? PayTime { get; set; }

        [DisplayName("付款時間")]

        public int? MemberId { get; set; }

        public string? MemberName { get; set; }
        public int Quantity { get; set; }

        [DisplayName("購買數量")]


        public int price { get; set; }
        [DisplayName("金額")]
        public decimal 小計 { get { return this.Quantity * this.price; } }
        public Product product { get; set; }
        public Order order { get; set; }
        public Member member { get; set; }

        public int? TypeId { get; set; }//商品類別編號
        public string? TypeName { get; set; }
        public string? Photo { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? TotalPrice { get; set; }
        public string? ShippingMethod {get ; set; }
        public string? PaymentMethod { get; set;}


    }
}
