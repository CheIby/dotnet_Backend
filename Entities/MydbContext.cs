using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace server.Entities;

public partial class MydbContext : DbContext
{
    public MydbContext()
    {
    }

    public MydbContext(DbContextOptions<MydbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderComment> OrderComments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.OrderId, "Order_id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "fk_Order_User_idx");

            entity.Property(e => e.Detail).HasMaxLength(500);
            entity.Property(e => e.OrderId)
                .HasMaxLength(45)
                .HasColumnName("Order_Id");
            entity.Property(e => e.ReceiveLocation)
                .HasMaxLength(45)
                .HasColumnName("Receive_location");
            entity.Property(e => e.Restaurant).HasMaxLength(45);
            entity.Property(e => e.UserId)
                .HasMaxLength(45)
                .HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasPrincipalKey(p => p.UserId)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_User");
        });

        modelBuilder.Entity<OrderComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order_comment");

            entity.HasIndex(e => e.OrderId, "fk_Order_Comment_Order1_idx");

            entity.HasIndex(e => e.UserId, "fk_Order_Comment_User1_idx");

            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.OrderId)
                .HasMaxLength(45)
                .HasColumnName("Order_Id");
            entity.Property(e => e.UserId)
                .HasMaxLength(45)
                .HasColumnName("User_Id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderComments)
                .HasPrincipalKey(p => p.OrderId)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Comment_Order1");

            entity.HasOne(d => d.User).WithMany(p => p.OrderComments)
                .HasPrincipalKey(p => p.UserId)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Comment_User1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserId, "User_id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Username, "Username_UNIQUE").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.UserId)
                .HasMaxLength(45)
                .HasColumnName("User_Id");
            entity.Property(e => e.Username).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
