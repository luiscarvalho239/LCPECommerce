using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPECommerce.Shared.Models
{
    public class Orders
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderName { get; set; } = "Order #1 - Product 1";
        public string OrderAddress { get; set; } = "Fake Street 1234";
        public string OrderEmail { get; set; } = "luigi@localhost.loc";
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public bool OrderStatus { get; set; } = true;
        public double Price { get; set; } = 1;
        public int Quantity { get; set; } = 1;
        public string ShippingAddress { get; set; } = "Fake Street 1234";
        public int ClientId { get; set; } = 1;
    }
}
