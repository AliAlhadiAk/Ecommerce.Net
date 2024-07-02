using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApiTesting.Model;

namespace WebApiTesting.DbContext
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       public   DbSet<Fan> Fans { get; set; }
       public  DbSet<Products> Products { get; set; }
       public DbSet<Sizes_Quantity> Sizes { get; set; }
       public DbSet<Transactions> Transactions { get; set; }
      public  DbSet<Inventory> Inventory { get; set; }
      public DbSet<AddToCart> AddToCart { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
