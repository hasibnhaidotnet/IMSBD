using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventorymanagementsystem.Models
{
    public class Quotation
    {
        public int Id { get; set; }

        [Required]
        public string QuotationNo { get; set; }

        [Required]
        public DateTime QuotationDate { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public ICollection<QuotationItem> Items { get; set; } = new List<QuotationItem>();

        public decimal TotalAmount
        {
            get => Items?.Sum(i => (decimal)(i.Quantity * i.Price)) ?? 0;
        }
    }
}
