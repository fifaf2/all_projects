using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WPFekz.Models;

public partial class ZxcContext : DbContext
{
    public ZxcContext()
    {
    }

    public ZxcContext(DbContextOptions<ZxcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Edinici> Edinicis { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Postavshiki> Postavshikis { get; set; }

    public virtual DbSet<Punkti> Punktis { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tovary> Tovaries { get; set; }

    public virtual DbSet<Tovzakaz> Tovzakazs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zakaz> Zakazs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=zxc", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.7.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.IdCategory)
                .ValueGeneratedNever()
                .HasColumnName("id_category");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(45)
                .HasColumnName("name_category");
        });

        modelBuilder.Entity<Edinici>(entity =>
        {
            entity.HasKey(e => e.IdEd).HasName("PRIMARY");

            entity.ToTable("edinici");

            entity.Property(e => e.IdEd)
                .ValueGeneratedNever()
                .HasColumnName("id_ed");
            entity.Property(e => e.NameEd)
                .HasMaxLength(45)
                .HasColumnName("name_ed");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.IdManufac).HasName("PRIMARY");

            entity.ToTable("manufacture");

            entity.Property(e => e.IdManufac)
                .ValueGeneratedNever()
                .HasColumnName("id_manufac");
            entity.Property(e => e.NameManufac)
                .HasMaxLength(100)
                .HasColumnName("name_manufac");
        });

        modelBuilder.Entity<Postavshiki>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PRIMARY");

            entity.ToTable("postavshiki");

            entity.Property(e => e.IdPost)
                .ValueGeneratedNever()
                .HasColumnName("id_post");
            entity.Property(e => e.NamePost)
                .HasMaxLength(105)
                .HasColumnName("name_post");
        });

        modelBuilder.Entity<Punkti>(entity =>
        {
            entity.HasKey(e => e.IdPunkti).HasName("PRIMARY");

            entity.ToTable("punkti");

            entity.Property(e => e.IdPunkti)
                .ValueGeneratedNever()
                .HasColumnName("id_punkti");
            entity.Property(e => e.Adress)
                .HasMaxLength(200)
                .HasColumnName("adress");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("id_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(200)
                .HasColumnName("name_role");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.IdStatus)
                .ValueGeneratedNever()
                .HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasMaxLength(45)
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<Tovary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tovary");

            entity.HasIndex(e => e.IdCategory, "id_category_idx");

            entity.HasIndex(e => e.IdEd, "id_ed_idx");

            entity.HasIndex(e => e.IdManufac, "id_manufac_idx");

            entity.HasIndex(e => e.IdPost, "id_post_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Articule)
                .HasMaxLength(140)
                .HasColumnName("articule");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdEd).HasColumnName("id_ed");
            entity.Property(e => e.IdManufac).HasColumnName("id_manufac");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.Kolvo).HasColumnName("kolvo");
            entity.Property(e => e.NameTovary)
                .HasMaxLength(300)
                .HasColumnName("name_tovary");
            entity.Property(e => e.OpisTovar)
                .HasMaxLength(415)
                .HasColumnName("opis_tovar");
            entity.Property(e => e.Photo)
                .HasMaxLength(45)
                .HasColumnName("photo");
            entity.Property(e => e.Scidka).HasColumnName("scidka");
            entity.Property(e => e.Sell).HasColumnName("sell");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Tovaries)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("id_category");

            entity.HasOne(d => d.IdEdNavigation).WithMany(p => p.Tovaries)
                .HasForeignKey(d => d.IdEd)
                .HasConstraintName("id_ed");

            entity.HasOne(d => d.IdManufacNavigation).WithMany(p => p.Tovaries)
                .HasForeignKey(d => d.IdManufac)
                .HasConstraintName("id_manufac");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Tovaries)
                .HasForeignKey(d => d.IdPost)
                .HasConstraintName("id_post");
        });

        modelBuilder.Entity<Tovzakaz>(entity =>
        {
            entity.HasKey(e => e.IdTovZakaz).HasName("PRIMARY");

            entity.ToTable("tovzakaz");

            entity.HasIndex(e => e.Id, "id_idx");

            entity.HasIndex(e => e.IdZakaz, "id_zakaz_idx");

            entity.Property(e => e.IdTovZakaz)
                .ValueGeneratedNever()
                .HasColumnName("id_tovZakaz");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdZakaz).HasColumnName("id_zakaz");
            entity.Property(e => e.Order).HasColumnName("order");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Tovzakazs)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("id");

            entity.HasOne(d => d.IdZakazNavigation).WithMany(p => p.Tovzakazs)
                .HasForeignKey(d => d.IdZakaz)
                .HasConstraintName("id_zakaz");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdRole, "id_role_idx");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("id_user");
            entity.Property(e => e.Fio)
                .HasMaxLength(200)
                .HasColumnName("FIO");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Login)
                .HasMaxLength(200)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("id_role");
        });

        modelBuilder.Entity<Zakaz>(entity =>
        {
            entity.HasKey(e => e.IdZakaz).HasName("PRIMARY");

            entity.ToTable("zakaz");

            entity.HasIndex(e => e.IdPunkt, "id_punkt_idx");

            entity.HasIndex(e => e.IdStatus, "id_status_idx");

            entity.HasIndex(e => e.IdUser, "id_user_idx");

            entity.Property(e => e.IdZakaz)
                .ValueGeneratedNever()
                .HasColumnName("id_zakaz");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DataDost).HasColumnName("data_dost");
            entity.Property(e => e.DateZakaz).HasColumnName("date_zakaz");
            entity.Property(e => e.IdPunkt).HasColumnName("id_punkt");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdPunktNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdPunkt)
                .HasConstraintName("id_punkt");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("id_status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Zakazs)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("id_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
