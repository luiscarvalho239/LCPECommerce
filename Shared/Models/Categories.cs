using System.ComponentModel.DataAnnotations.Schema;

namespace LCPECommerce.Shared.Models
{
    public class Categories
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "Tutoriais";
        public string Desc { get; set; } = "tutoriais";
        public string Image { get; set; } = "tutoriais.png";
    }
}
