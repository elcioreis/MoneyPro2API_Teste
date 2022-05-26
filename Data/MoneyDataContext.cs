using Microsoft.EntityFrameworkCore;
using MoneyPro2.Data.Mappings;
using MoneyPro2.Models;

namespace MoneyPro2.Data;
public class MoneyDataContext : DbContext
{
    public MoneyDataContext(DbContextOptions<MoneyDataContext> context)
        : base(context) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Login> Logins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new LoginMap());

        // Insere os "Roles" ao criar o banco
        modelBuilder.Entity<Role>()
            .HasData(
                 new Role
                 {
                     Id = 1,
                     Name = "admin",
                     Slug = "admin"
                 },
                new Role
                {
                    Id = 2,
                    Name = "user",
                    Slug = "user"
                }
            );
    }
}