using firstProject.Models;
using Microsoft.EntityFrameworkCore;
namespace firstProject.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
        : base(dbContextOptions)
        { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
