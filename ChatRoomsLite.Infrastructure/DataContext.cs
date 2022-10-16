using ChatRoomsLite.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomsLite.Infrastructure;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User>? Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();
    }
}