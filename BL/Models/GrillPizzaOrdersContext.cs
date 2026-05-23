using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BL.Models;

public partial class GrillPizzaOrdersContext : DbContext
{
    public GrillPizzaOrdersContext()
    {
    }

    public GrillPizzaOrdersContext(DbContextOptions<GrillPizzaOrdersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergen> Allergens { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LogLevel> LogLevels { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DatabazikConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allergen__3214EC07536A60EF");

            entity.ToTable("Allergen");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC0799CB658A");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Food__3214EC0702D67A9F");

            entity.ToTable("Food");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Category).WithMany(p => p.Foods)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Food__CategoryId__52593CB8");

            entity.HasMany(d => d.Allergens).WithMany(p => p.Foods)
                .UsingEntity<Dictionary<string, object>>(
                    "FoodAllergen",
                    r => r.HasOne<Allergen>().WithMany()
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FoodAller__Aller__5629CD9C"),
                    l => l.HasOne<Food>().WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__FoodAller__FoodI__5535A963"),
                    j =>
                    {
                        j.HasKey("FoodId", "AllergenId").HasName("PK__FoodAlle__64350AD2C1B8991D");
                        j.ToTable("FoodAllergen");
                    });
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC0762AB1C1F");

            entity.ToTable("Log");

            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.LogLevel).WithMany(p => p.Logs)
                .HasForeignKey(d => d.LogLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log__LogLevelId__656C112C");
        });

        modelBuilder.Entity<LogLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LogLevel__3214EC076292ADE1");

            entity.ToTable("LogLevel");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07482F4F9A");

            entity.ToTable("Order");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__PaymentId__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserId__5AEE82B9");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC0724C91969");

            entity.ToTable("OrderItem");

            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Food).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__FoodI__5FB337D6");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__5EBF139D");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07C764204B");

            entity.ToTable("Payment");

            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC076E34F3F4");

            entity.ToTable("Role");

            entity.Property(e => e.RoleTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07D70B2D6A");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.PwdHash).HasMaxLength(255);
            entity.Property(e => e.PwdSalt).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleId__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
