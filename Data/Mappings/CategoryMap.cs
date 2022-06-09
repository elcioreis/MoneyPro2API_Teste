using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;
public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");

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

        builder.Property(x => x.CategoryParentId)
            .IsRequired(false)
            .HasColumnName("CategoryParentId")
            .HasColumnType("INT");

        builder.Property(x => x.CategoryGroupId)
            .IsRequired(false)
            .HasColumnName("CategoryGroupId")
            .HasColumnType("INT");

        builder.Property(x => x.CrdDeb)
            .IsRequired(false)
            .HasColumnName("CrdDeb")
            .HasColumnType("CHAR")
            .HasMaxLength(1);

        builder.Property(x => x.VisualOrder)
            .IsRequired(false)
            .HasColumnName("VisualOrder")
            .HasColumnType("INT");

        builder.Property(x => x.Fixed)
            .IsRequired()
            .HasColumnName("Fixed")
            .HasColumnType("BIT");

        builder.Property(x => x.Active)
            .IsRequired()
            .HasColumnName("Active")
            .HasColumnType("BIT");

        builder.Property(x => x.System)
            .IsRequired()
            .HasColumnName("System")
            .HasColumnType("BIT");

        builder.HasIndex(x => new { x.UserId, x.Name }, "IX_Category_UserId_Name").IsUnique();

        // Exemplo de tabela auto referenciada
        // Auto referência do Campo Category.CategoryParentId para Category.Id
        builder
            .HasMany(x => x.Children)
            .WithOne(x => x.CategoryParent)
            .HasForeignKey("CategoryParentId")
            .HasConstraintName("FK_Category_CategoryParent")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
