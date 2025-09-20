using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventorymanagementsystem.Models
{
    public enum TransactionType
    {
        Receive = 0, 
        Payment = 1  
    }

    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public int? SaleId { get; set; }
        public Sale Sale { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; } // enum: Receive / Payment

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }
        public int? PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public string? Note { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }
    }
}
