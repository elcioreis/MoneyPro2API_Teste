using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;
public class EntryMap : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder.ToTable("Entry");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
          .HasColumnName("Id")
          .HasColumnType("INT")
          .ValueGeneratedOnAdd()
          .UseIdentityColumn();

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("UserId")
            .HasColumnType("INT");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(40);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnName("Description")
            .HasColumnType("VARCHAR")
            .HasMaxLength(150);

        builder.Property(x => x.Active)
            .IsRequired()
            .HasColumnName("Active")
            .HasColumnType("BIT")
            .HasDefaultValueSql("1");

        builder.Property(x => x.System)
            .IsRequired()
            .HasColumnName("System")
            .HasColumnType("BIT")
            .HasDefaultValueSql("0");

        builder.HasIndex(x => new { x.UserId, x.Name }, "IX_Entry_UserId_Name").IsUnique();
    }
}