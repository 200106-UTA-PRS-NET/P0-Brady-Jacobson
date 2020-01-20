using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain.Models
{
    public partial class PizzaDBContext : DbContext
    {
        public PizzaDBContext()
        {
        }

        public PizzaDBContext(DbContextOptions<PizzaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizzas> Pizzas { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }
        public virtual DbSet<Toppings> Toppings { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
                // Deleted Secret String
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Orders__0809337D22E4B8F6");

                entity.ToTable("Orders", "Pizza_Schema");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("orderTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PizzaAmount).HasColumnName("pizzaAmount");

                entity.Property(e => e.StoreId).HasColumnName("storeID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__storeID__1CBC4616");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__userID__1DB06A4F");
            });

            modelBuilder.Entity<Pizzas>(entity =>
            {
                entity.HasKey(e => e.PizzaId)
                    .HasName("PK__Pizzas__4D4C90CF900AAF91");

                entity.ToTable("Pizzas", "Pizza_Schema");

                entity.Property(e => e.PizzaId).HasColumnName("pizzaID");

                entity.Property(e => e.Crust)
                    .IsRequired()
                    .HasColumnName("crust")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.PizzaCost)
                    .HasColumnName("pizzaCost")
                    .HasColumnType("money");

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasColumnName("size")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Pizzas)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pizzas__orderID__208CD6FA");
            });

            modelBuilder.Entity<Stores>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("PK__Stores__1EA716331174CC67");

                entity.ToTable("Stores", "Pizza_Schema");

                entity.HasIndex(e => e.StoreName)
                    .HasName("UQ__Stores__0E3E451D96309586")
                    .IsUnique();

                entity.Property(e => e.StoreId).HasColumnName("storeID");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasColumnName("storeCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasColumnName("storeName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Toppings>(entity =>
            {
                entity.HasKey(e => e.ToppingId)
                    .HasName("PK__Toppings__AAE1560922457C0E");

                entity.ToTable("Toppings", "Pizza_Schema");

                entity.Property(e => e.ToppingId).HasColumnName("toppingID");

                entity.Property(e => e.PizzaId).HasColumnName("pizzaID");

                entity.Property(e => e.Topping)
                    .HasColumnName("topping")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.Toppings)
                    .HasForeignKey(d => d.PizzaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Toppings__pizzaI__236943A5");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__CB9A1CDF1FD5E7CF");

                entity.ToTable("Users", "Pizza_Schema");

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__Users__66DCF95C7BBEC8C5")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.StoreId).HasColumnName("storeID");

                entity.Property(e => e.StoreTime)
                    .HasColumnName("storeTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasColumnName("userCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Users__storeID__19DFD96B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
