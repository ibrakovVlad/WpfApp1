using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MyDbContext : DbContext
    {
        private static MyDbContext _context;

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }

        public static MyDbContext GetContext()
        {
            if (_context == null)
                _context = new MyDbContext();

            return _context;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"data source=DESKTOP-MAEGUNQ;Initial Catalog = Market; Integrated Security = True; ";
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
