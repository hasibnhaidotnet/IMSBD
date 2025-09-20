namespace Inventorymanagementsystem.Models
{
    public class DamageReturn
    {
        public int Id { get; set; }

        public int PurchaseItemId { get; set; }
        public PurchaseItem PurchaseItem { get; set; }

        public double Quantity { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Note { get; set; }
    }

}
