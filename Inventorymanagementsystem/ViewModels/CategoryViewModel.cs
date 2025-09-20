using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }

}
