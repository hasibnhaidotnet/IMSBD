namespace Inventorymanagementsystem.Models
{
    public class Damage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public DateTime DamageDate { get; set; }
        public string Note { get; set; }
    }

}
