using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MoneyManager.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoneyManager.Data
{
    public partial class MoneyManagerContext : DbContext
    {
        public MoneyManagerContext()
        {
        }

        public MoneyManagerContext(DbContextOptions<MoneyManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BankOperation> BankOperation { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Deposit> Deposit { get; set; }
        public virtual DbSet<DepositOperation> DepositOperation { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=MoneyManager;Username=postgres;Password=1111");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AccountBalance)
                    .HasColumnName("account_balance")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("account_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_currency_id_fkey");
            });

            modelBuilder.Entity<BankOperation>(entity =>
            {
                entity.HasKey(e => e.OperationId)
                    .HasName("bank_operation_pkey");

                entity.ToTable("bank_operation");

                entity.Property(e => e.OperationId).HasColumnName("operation_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.OperationAmount)
                    .HasColumnName("operation_amount")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.OperationDate)
                    .HasColumnName("operation_date")
                    .HasColumnType("date");

                entity.Property(e => e.OperationType)
                    .IsRequired()
                    .HasColumnName("operation_type")
                    .HasMaxLength(50);

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.BankOperation)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("bank_operation_account_id_fkey");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.BankOperation)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bank_operation_subcategory_id_fkey");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("currency");

                entity.HasIndex(e => e.CurrencyCode)
                    .HasName("currency_currency_code_key")
                    .IsUnique();

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasColumnName("currency_code")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.ToTable("deposit");

                entity.Property(e => e.DepositId).HasColumnName("deposit_id");

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.DepositAmount)
                    .HasColumnName("deposit_amount")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.InterestRate)
                    .HasColumnName("interest_rate")
                    .HasColumnType("numeric(5,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Deposit)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("deposit_currency_id_fkey");
            });

            modelBuilder.Entity<DepositOperation>(entity =>
            {
                entity.ToTable("deposit_operation");

                entity.Property(e => e.DepositOperationId).HasColumnName("deposit_operation_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.DepositId).HasColumnName("deposit_id");

                entity.Property(e => e.OperationAmount)
                    .HasColumnName("operation_amount")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.OperationDate)
                    .HasColumnName("operation_date")
                    .HasColumnType("date");

                entity.Property(e => e.OperationType)
                    .IsRequired()
                    .HasColumnName("operation_type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.DepositOperation)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("deposit_operation_account_id_fkey");

                entity.HasOne(d => d.Deposit)
                    .WithMany(p => p.DepositOperation)
                    .HasForeignKey(d => d.DepositId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("deposit_operation_deposit_id_fkey");
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.ToTable("subcategory");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.SubcategoryName)
                    .IsRequired()
                    .HasColumnName("subcategory_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subcategory_category_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
