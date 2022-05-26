using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Tabela
        builder.ToTable("Role");

        // Chave primária
        builder.HasKey(x => x.Id);

        // Identidade
        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("Id")
            .HasColumnType("INT");

        // Propriedades

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(10);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(10);

        // Índices
        builder.HasIndex(x => x.Name, "IX_Role_Name").IsUnique();
        builder.HasIndex(x => x.Slug, "IX_Role_Slug").IsUnique();
    }
}