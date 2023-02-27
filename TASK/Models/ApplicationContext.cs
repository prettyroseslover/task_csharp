using Microsoft.EntityFrameworkCore;

namespace TASK.Models;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        // (DbContextOptions<ApplicationContext> options) : base(options)
    }
    public DbSet<Value> Values { get; set; }
    public DbSet<Result> Results { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationdb;Trusted_Connection=True;");
    }
}