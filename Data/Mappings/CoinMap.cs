using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;
public class CoinMap : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        builder.ToTable("Coin");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("INT")
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(25);

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasColumnName("Symbol")
            .HasColumnType("VARCHAR")
            .HasMaxLength(10);

        builder.Property(x => x.Default)
            .IsRequired()
            .HasColumnName("Default")
            .HasColumnType("BIT");

        builder.Property(x => x.Virtual)
            .IsRequired()
            .HasColumnName("Virtual")
            .HasColumnType("BIT");

        builder.Property(x => x.Active)
            .IsRequired()
            .HasColumnName("Active")
            .HasColumnType("BIT");

        builder.HasIndex(x => x.Symbol, "IX_Coin_Symbol").IsUnique();
        builder.HasIndex(x => x.Name, "IX_Coin_Name").IsUnique();
    }
}
