using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Market.Models
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            var connectionString = "Server=IP;User Id=USER;Password=PASSWORD;Database=Market;Encrypt=false";
            optionsBuilder.UseSqlServer(connectionString);
            return new MyDbContext();
        }
    }
}
