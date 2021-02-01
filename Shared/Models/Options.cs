using System.ComponentModel.DataAnnotations.Schema;

namespace LCPECommerce.Shared.Models
{
    public class Options
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OptionName { get; set; } = "Pending...";
    }
}
