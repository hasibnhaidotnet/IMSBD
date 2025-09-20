using Microsoft.EntityFrameworkCore;
using Inventorymanagementsystem.Models;

namespace Inventorymanagementsystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Unit> Units { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationItem> QuotationItems { get; set; }
        public DbSet<Damage> Damages { get; set; }
        public DbSet<DamageReturn> DamageReturns { get; set; }
        public DbSet<SalesReturn> SalesReturns { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //poddari korlam 
            // 🔥 সব DateTime property কে timestamp without time zone বানিয়ে দিচ্ছি
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var dateTimeProperties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in dateTimeProperties)
                {
                    property.SetColumnType("timestamp without time zone");
                }
            }
            // Prevent cascade delete for Product in PurchaseItem
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Product)
                .WithMany()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete for Product in QuotationItem
            modelBuilder.Entity<QuotationItem>()
                .HasOne(qi => qi.Product)
                .WithMany()
                .HasForeignKey(qi => qi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.RelatedPurchaseItem)
                .WithMany()
                .HasForeignKey(si => si.RelatedPurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict);


            // Prevent cascade delete for RelatedPurchaseItem in SaleItem
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.RelatedPurchaseItem)
                .WithMany()
                .HasForeignKey(si => si.RelatedPurchaseItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete for Product in Damage
            modelBuilder.Entity<Damage>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
