using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.ViewModels
{
    public class UnitViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Unit Name")]
        public string Name { get; set; }
    }

}
