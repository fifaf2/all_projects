using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace api_floower.Models;

public partial class FlooresContext : DbContext
{
    public FlooresContext()
    {
    }

    public FlooresContext(DbContextOptions<FlooresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Postavshik> Postavshiks { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productorder> Productorders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=floores;user=root;password=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.7.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.IdUser, "a_idx");

            entity.HasIndex(e => e.IdStatus, "b_idx");

            entity.Property(e => e.IdOrder)
                .ValueGeneratedNever()
                .HasColumnName("id_order");
            entity.Property(e => e.CodePoly).HasColumnName("code_poly");
            entity.Property(e => e.DataZakaz)
                .HasColumnType("datetime")
                .HasColumnName("data_zakaz");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("b");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("a");
        });

        modelBuilder.Entity<Postavshik>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PRIMARY");

            entity.ToTable("postavshik");

            entity.Property(e => e.IdPost)
                .ValueGeneratedNever()
                .HasColumnName("id_post");
            entity.Property(e => e.NamePost)
                .HasMaxLength(405)
                .HasColumnName("name_post");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.IdPost, "as_idx");

            entity.Property(e => e.IdProduct)
                .ValueGeneratedNever()
                .HasColumnName("id_product");
            entity.Property(e => e.Description)
                .HasMaxLength(405)
                .HasColumnName("description");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.Kolvo).HasColumnName("kolvo");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(405)
                .HasColumnName("name_product");
            entity.Property(e => e.Sell).HasColumnName("sell");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdPost)
                .HasConstraintName("as");
        });

        modelBuilder.Entity<Productorder>(entity =>
        {
            entity.HasKey(e => e.IdProductorders).HasName("PRIMARY");

            entity.ToTable("productorders");

            entity.HasIndex(e => e.IdProduct, "a_idx");

            entity.HasIndex(e => e.IdOrder, "b_idx");

            entity.Property(e => e.IdProductorders)
                .ValueGeneratedNever()
                .HasColumnName("id_productorders");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Kolvo).HasColumnName("kolvo");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Productorders)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("df");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Productorders)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("ab");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("id_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(100)
                .HasColumnName("name_role");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PRIMARY");

            entity.ToTable("statuses");

            entity.Property(e => e.IdStatus)
                .ValueGeneratedNever()
                .HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasMaxLength(45)
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdRole, "ld_idx");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("id_user");
            entity.Property(e => e.Fio)
                .HasMaxLength(45)
                .HasColumnName("FIO");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("ld");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
