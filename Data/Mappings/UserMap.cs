using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;
public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Identificacação da tabela
        builder.ToTable("User");

        // Chave primaria
        builder.HasKey(x => x.Id);

        // Identidade
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        // Propriedade das colunas

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.Property(x => x.ControlStart)
            .IsRequired()
            .HasColumnName("ControlStart")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnName("PasswordHash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(64);

        builder.Property(x => x.Image)
            .IsRequired(false);

        // Índices
        builder.HasIndex(x => x.Email, "IX_User_Email").IsUnique();
        builder.HasIndex(x => x.Slug, "IX_User_Slug").IsUnique();

        //Relacionamentos
        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId"),
                user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId"));

        builder
            .HasMany(x => x.Logins)
            .WithOne(x => x.User)
            .HasForeignKey("UserId")
            .HasConstraintName("Fk_Login_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.InstitutionTypes)
            .WithOne(x => x.User)
            .HasForeignKey("UserId")
            .HasConstraintName("Fk_InstitutionTypes_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Institutions)
            .WithOne(x => x.User)
            .HasForeignKey("UserId")
            .HasConstraintName("Fk_Institution_User")
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.Entries)
            .WithOne(x => x.User)
            .HasForeignKey("UserId")
            .HasConstraintName("Fk_Entry_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.CategoryGroups)
            .WithOne(x => x.User)
            .HasForeignKey("UserId")
            .HasConstraintName("Fk_CategoryGroups_User")
            .OnDelete(DeleteBehavior.Cascade);
    }
}