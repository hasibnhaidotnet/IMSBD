using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
