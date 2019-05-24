﻿using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bank.Persistence
{
    public partial class BankAppDataContext : IdentityDbContext<IdentityUser>, IBankDbContext
    {
        public BankAppDataContext(DbContextOptions<BankAppDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Disposition> Dispositions { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<PermenentOrder> PermenentOrder { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK_account");

                entity.Property(e => e.Balance).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.CardId);

                entity.Property(e => e.Ccnumber)
                    .IsRequired()
                    .HasColumnName("CCNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Cctype)
                    .IsRequired()
                    .HasColumnName("CCType")
                    .HasMaxLength(50);

                entity.Property(e => e.Cvv2)
                    .IsRequired()
                    .HasColumnName("CVV2")
                    .HasMaxLength(10);

                entity.Property(e => e.Issued).HasColumnType("date");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Disposition)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.DispositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cards_Dispositions");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.Emailaddress).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.Givenname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NationalId).HasMaxLength(20);

                entity.Property(e => e.Streetaddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Telephonecountrycode).HasMaxLength(10);

                entity.Property(e => e.Telephonenumber).HasMaxLength(25);

                entity.Property(e => e.Zipcode)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Disposition>(entity =>
            {
                entity.HasKey(e => e.DispositionId)
                    .HasName("PK_disposition");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Dispositions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dispositions_Accounts");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Dispositions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dispositions_Customers");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.LoanId)
                    .HasName("PK_loan");

                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Payments).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loans_Accounts");
            });

            modelBuilder.Entity<PermenentOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.AccountTo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.BankTo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PermenentOrders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermenentOrder_Accounts");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK_trans2");

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Balance).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Bank).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Operation)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Symbol).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Accounts");
            });
        }
    }
}
