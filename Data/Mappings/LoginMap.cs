using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Models;

namespace MoneyPro2.Data.Mappings;
public class LoginMap : IEntityTypeConfiguration<Login>
{
    public void Configure(EntityTypeBuilder<Login> builder)
    {
        // Identificacação da tabela
        builder.ToTable("Login");

        // Chave primaria
        builder.HasKey(x => x.Id);

        // Identidade
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        // Propriedades
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("UserId")
            .HasColumnType("int");

        builder.Property(x => x.LoginDate)
            .HasColumnName("LoginDate")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.IpAddress)
            .IsRequired(false)
            .HasColumnName("IpAddress")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

        // Índices
        builder.HasIndex(x => new { x.UserId, x.LoginDate }, "IX_Login_UserId_LoginDate").IsUnique();
    }
}