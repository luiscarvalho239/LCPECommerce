using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPECommerce.Shared.Models
{
    public class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "Product 1";
        public double Price { get; set; } = 1;
        public string Desc { get; set; } = "Desc of Product 1";
        public string Image { get; set; } = "default.png";
        public string Category { get; set; } = "others";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int Stock { get; set; } = 1;
        public int ProductId { get; set; } = 1;
        public int CategoryId { get; set; } = 1;
        public int OptionId { get; set; } = 1;
    }
}
