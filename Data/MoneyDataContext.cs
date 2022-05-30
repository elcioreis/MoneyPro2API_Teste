﻿using Microsoft.EntityFrameworkCore;
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
    public DbSet<Coin> Coins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new LoginMap());
        modelBuilder.ApplyConfiguration(new CoinMap());

        // Insere os "Roles" ao criar o banco
        modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = "admin", Slug = "admin" },
                new Role { Id = 2, Name = "user", Slug = "user" }
            );

        // Insere o Real Brasileiro como moeda padrão no sistema
        modelBuilder.Entity<Coin>()
            .HasData(new Coin { Id = 1, Nickname = "Real Brasileiro", Symbol = "R$", Default = true, Virtual = false, Active = true });
    }
}