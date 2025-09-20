using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public enum ClientType
    {
        Supplier = 1,
        Buyer = 2
    }

    public class Client
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public ClientType ClientType { get; set; }

        public ICollection<Purchase>? Purchases { get; set; }
        public ICollection<Sale>? Sales { get; set; }
    }

}
