using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public ICollection<SubCategory>? SubCategories { get; set; }
        public ICollection<Product>? Products { get; set; }
    }

}
