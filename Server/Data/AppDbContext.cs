using Microsoft.EntityFrameworkCore;
using LCPECommerce.Shared.Models;

namespace LCPECommerce.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Clients> Client { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<Orders> Order { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Options> Option { get; set; }
    }
}
