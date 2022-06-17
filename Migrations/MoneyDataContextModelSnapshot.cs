﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyPro2.Data;

#nullable disable

namespace MoneyPro2.Migrations
{
    [DbContext(typeof(MoneyDataContext))]
    partial class MoneyDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MoneyPro2.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("BIT")
                        .HasColumnName("Active");

                    b.Property<int?>("CategoryGroupId")
                        .HasColumnType("INT")
                        .HasColumnName("CategoryGroupId");

                    b.Property<int?>("CategoryParentId")
                        .HasColumnType("INT")
                        .HasColumnName("CategoryParentId");

                    b.Property<string>("CrdDeb")
                        .HasMaxLength(1)
                        .HasColumnType("CHAR(1)")
                        .HasColumnName("CrdDeb");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("Description");

                    b.Property<bool>("Fixed")
                        .HasColumnType("BIT")
                        .HasColumnName("Fixed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)")
                        .HasColumnName("Name");

                    b.Property<bool>("System")
                        .HasColumnType("BIT")
                        .HasColumnName("System");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("UserId");

                    b.Property<int?>("VisualOrder")
                        .HasColumnType("INT")
                        .HasColumnName("VisualOrder");

                    b.HasKey("Id");

                    b.HasIndex("CategoryGroupId");

                    b.HasIndex("CategoryParentId");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_Category_UserId_Name")
                        .IsUnique();

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.CategoryGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasColumnName("Active")
                        .HasDefaultValueSql("1");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)")
                        .HasColumnName("Name");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_CategoryGroup_UserId_Name")
                        .IsUnique();

                    b.ToTable("CategoryGroup", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.Coin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("BIT")
                        .HasColumnName("Active");

                    b.Property<bool>("Default")
                        .HasColumnType("BIT")
                        .HasColumnName("Default");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("VARCHAR(25)")
                        .HasColumnName("Name");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)")
                        .HasColumnName("Symbol");

                    b.Property<bool>("Virtual")
                        .HasColumnType("BIT")
                        .HasColumnName("Virtual");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_Coin_Name")
                        .IsUnique();

                    b.HasIndex(new[] { "Symbol" }, "IX_Coin_Symbol")
                        .IsUnique();

                    b.ToTable("Coin", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Default = true,
                            Name = "Real Brasileiro",
                            Symbol = "R$",
                            Virtual = false
                        });
                });

            modelBuilder.Entity("MoneyPro2.Models.Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasColumnName("Active")
                        .HasDefaultValueSql("1");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)")
                        .HasColumnName("Name");

                    b.Property<bool>("System")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasColumnName("System")
                        .HasDefaultValueSql("0");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_Entry_UserId_Name")
                        .IsUnique();

                    b.ToTable("Entry", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.Institution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasColumnName("Active")
                        .HasDefaultValueSql("1");

                    b.Property<int?>("BankNumber")
                        .HasColumnType("INT")
                        .HasColumnName("BankNumber");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("Description");

                    b.Property<int>("InstitutionTypeId")
                        .HasColumnType("INT")
                        .HasColumnName("InstitutionTypeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)")
                        .HasColumnName("Name");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionTypeId");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_Institution_UserId_Name")
                        .IsUnique();

                    b.ToTable("Institution", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.InstitutionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasColumnName("Active")
                        .HasDefaultValueSql("1");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR(150)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("VARCHAR(40)")
                        .HasColumnName("Name");

                    b.Property<int>("UserId")
                        .HasColumnType("INT")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "Name" }, "IX_InstitutionType_UserId_Name")
                        .IsUnique();

                    b.ToTable("InstitutionType", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("IpAddress")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR(20)")
                        .HasColumnName("IpAddress");

                    b.Property<DateTime>("LoginDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasColumnName("LoginDate")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "LoginDate" }, "IX_Login_UserId_LoginDate")
                        .IsUnique();

                    b.ToTable("Login", (string)null);
                });

            modelBuilder.Entity("MoneyPro2.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INT")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)")
                        .HasColumnName("Name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR(10)")
                        .HasColumnName("Slug");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_Role_Name")
                        .IsUnique();

                    b.HasIndex(new[] { "Slug" }, "IX_Role_Slug")
                        .IsUnique();

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin",
                            Slug = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "user",
                            Slug = "user"
                        });
                });

            modelBuilder.Entity("MoneyPro2.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ControlStart")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("DATETIME")
                        .HasColumnName("ControlStart")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("VARCHAR(160)")
                        .HasColumnName("Email");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("NVARCHAR(80)")
                        .HasColumnName("Name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR(64)")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("VARCHAR(160)")
                        .HasColumnName("Slug");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "IX_User_Email")
                        .IsUnique();

                    b.HasIndex(new[] { "Slug" }, "IX_User_Slug")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("INT");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("MoneyPro2.Models.Category", b =>
                {
                    b.HasOne("MoneyPro2.Models.CategoryGroup", "CategoryGroup")
                        .WithMany("Categories")
                        .HasForeignKey("CategoryGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Category_CategoryGroup");

                    b.HasOne("MoneyPro2.Models.Category", "CategoryParent")
                        .WithMany("Children")
                        .HasForeignKey("CategoryParentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Category_CategoryParent");

                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_Category_User");

                    b.Navigation("CategoryGroup");

                    b.Navigation("CategoryParent");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoneyPro2.Models.CategoryGroup", b =>
                {
                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("CategoryGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_CategoryGroup_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoneyPro2.Models.Entry", b =>
                {
                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("Entries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_Entry_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoneyPro2.Models.Institution", b =>
                {
                    b.HasOne("MoneyPro2.Models.InstitutionType", "InstitutionType")
                        .WithMany("Institutions")
                        .HasForeignKey("InstitutionTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_Institution_InstitutionType");

                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("Institutions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_Institution_User");

                    b.Navigation("InstitutionType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoneyPro2.Models.InstitutionType", b =>
                {
                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("InstitutionTypes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Fk_InstitutionTypes_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoneyPro2.Models.Login", b =>
                {
                    b.HasOne("MoneyPro2.Models.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Fk_Login_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("MoneyPro2.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_RoleId");

                    b.HasOne("MoneyPro2.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_UserId");
                });

            modelBuilder.Entity("MoneyPro2.Models.Category", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("MoneyPro2.Models.CategoryGroup", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("MoneyPro2.Models.InstitutionType", b =>
                {
                    b.Navigation("Institutions");
                });

            modelBuilder.Entity("MoneyPro2.Models.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("CategoryGroups");

                    b.Navigation("Entries");

                    b.Navigation("InstitutionTypes");

                    b.Navigation("Institutions");

                    b.Navigation("Logins");
                });
#pragma warning restore 612, 618
        }
    }
}
