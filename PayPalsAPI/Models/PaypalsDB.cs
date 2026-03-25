using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PayPalsAPI.Models;

public partial class PaypalsDB : DbContext
{
    public PaypalsDB()
    {
    }

    public PaypalsDB(DbContextOptions<PaypalsDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<ShoppingListPerson> ShoppingListPeople { get; set; }

    public virtual DbSet<ShoppingListProduct> ShoppingListProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=paypals_db;user=root;password=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PRIMARY");

            entity.ToTable("person");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.IsAlcoholic).HasColumnName("is_alcoholic");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.ShoppingListId).HasName("PRIMARY");

            entity.ToTable("shopping_list");

            entity.HasIndex(e => e.PayerId, "fk_shopping_list_payer");

            entity.Property(e => e.ShoppingListId).HasColumnName("shopping_list_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PayerId).HasColumnName("payer_id");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");

            entity.HasOne(d => d.Payer).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.PayerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_shopping_list_payer");
        });

        modelBuilder.Entity<ShoppingListPerson>(entity =>
        {
            entity.HasKey(e => e.ShoppingListPersonId).HasName("PRIMARY");

            entity.ToTable("shopping_list_person");

            entity.HasIndex(e => e.PersonId, "fk_slp_person");

            entity.HasIndex(e => new { e.ShoppingListId, e.PersonId }, "uq_slp").IsUnique();

            entity.Property(e => e.ShoppingListPersonId).HasColumnName("shopping_list_person_id");
            entity.Property(e => e.PaysForAlcohol)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("pays_for_alcohol");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.ShoppingListId).HasColumnName("shopping_list_id");

            entity.HasOne(d => d.Person).WithMany(p => p.ShoppingListPeople)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_slp_person");

            entity.HasOne(d => d.ShoppingList).WithMany(p => p.ShoppingListPeople)
                .HasForeignKey(d => d.ShoppingListId)
                .HasConstraintName("fk_slp_list");
        });

        modelBuilder.Entity<ShoppingListProduct>(entity =>
        {
            entity.HasKey(e => e.ShoppingListProductId).HasName("PRIMARY");

            entity.ToTable("shopping_list_product");

            entity.HasIndex(e => e.ProductId, "fk_slpr_product");

            entity.HasIndex(e => new { e.ShoppingListId, e.ProductId }, "uq_slpr").IsUnique();

            entity.Property(e => e.ShoppingListProductId).HasColumnName("shopping_list_product_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("'1'")
                .HasColumnName("quantity");
            entity.Property(e => e.ShoppingListId).HasColumnName("shopping_list_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingListProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_slpr_product");

            entity.HasOne(d => d.ShoppingList).WithMany(p => p.ShoppingListProducts)
                .HasForeignKey(d => d.ShoppingListId)
                .HasConstraintName("fk_slpr_list");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
