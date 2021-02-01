using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPECommerce.Shared.Models
{
    public class Clients
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; } = "luigi";

        [Required(ErrorMessage = "O email do cliente é obrigatório")]
        public string Email { get; set; } = "luigi@localhost.loc";

        [Required(ErrorMessage = "A palavra-passe do cliente é obrigatório")]
        public string Password { get; set; } = "luigi1234";

        public string FullName { get; set; } = "Luis Carvalho";
        public string BillingAddress { get; set; } = "Fake Street 1234";
        public string ShippingAddress { get; set; } = "Fake Street 1234";
        public string Country { get; set; } = "Portugal";
        public string Phone { get; set; } = "1234567890";
        public string Token { get; set; }
        public bool IsDeleting { get; set; }
    }
}
