using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public ICollection<Product>? Products { get; set; }
    }

}
