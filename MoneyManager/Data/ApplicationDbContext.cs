﻿using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;

namespace MoneyManager.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BankTransaction> BankTransaction { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ClosedDeposits> ClosedDeposits { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Deposit> Deposit { get; set; }
        public virtual DbSet<DepositOperation> DepositOperation { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }
        public virtual DbSet<TotalDepositsLastMonth> TotalDepositsLastMonth { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                optionsBuilder.UseNpgsql(connectionString);
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

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_currency_id_fkey");
            });

            modelBuilder.Entity<BankTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("bank_transaction_pkey");

                entity.ToTable("bank_transaction");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transaction_amount")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("transaction_type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.BankTransaction)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bank_transaction_account_id_fkey");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.BankTransaction)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bank_transaction_subcategory_id_fkey");
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

            modelBuilder.Entity<ClosedDeposits>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("closed_deposits");

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.DepositAmount)
                    .HasColumnName("deposit_amount")
                    .HasColumnType("numeric(10,2)");

                entity.Property(e => e.DepositId).HasColumnName("deposit_id");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.InterestRate)
                    .HasColumnName("interest_rate")
                    .HasColumnType("numeric(5,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");
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

            modelBuilder.Entity<TotalDepositsLastMonth>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("total_deposits_last_month");

                entity.Property(e => e.TotalDepositAmount)
                    .HasColumnName("total_deposit_amount")
                    .HasColumnType("numeric");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
