﻿using System;
using BankApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AspBankApp.Repository
{
    public partial class BankAppDataContext : DbContext
    {
        public BankAppDataContext()
        {
        }

        public BankAppDataContext(DbContextOptions<BankAppDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
       // public virtual DbSet<Disposition> Dispositions { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Created).HasColumnType("date");

                //entity.Property(e => e.Frequency)
                //    .IsRequired()
                //    .HasMaxLength(50);

                entity.HasOne(d => d.AccountTypes)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypesId)
                    .HasConstraintName("FK_Accounts_AccountTypes");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Ccnumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CCNumber");

                entity.Property(e => e.Cctype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CCType");

                entity.Property(e => e.Cvv2)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("CVV2");

                entity.Property(e => e.Issued).HasColumnType("date");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                //entity.HasOne(d => d.Disposition)
                //    .WithMany(p => p.Cards)
                //    .HasForeignKey(d => d.DispositionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Cards_Dispositions");
            });

            modelBuilder.Entity<Customer>(entity =>
            {


                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.City)
                    
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    
                    .HasMaxLength(100);

                entity.Property(e => e.CountryCode)
                    
                    .HasMaxLength(2);

                entity.Property(e => e.Emailaddress).HasMaxLength(100);

                entity.Property(e => e.Gender)
                    
                    .HasMaxLength(6);

                entity.Property(e => e.Givenname)
                    
                    .HasMaxLength(100);

                entity.Property(e => e.Streetaddress)
                    
                    .HasMaxLength(100);

                entity.Property(e => e.Surname)
                    
                    .HasMaxLength(100);

                entity.Property(e => e.Telephonecountrycode).HasMaxLength(10);

                entity.Property(e => e.Telephonenumber).HasMaxLength(25);

                entity.Property(e => e.Zipcode)
                   
                    .HasMaxLength(15);
            });

            //modelBuilder.Entity<Disposition>(entity =>
            //{
            //    //entity.Property(e => e.Type)
            //    //    .IsRequired()
            //    //    .HasMaxLength(50);

            //    //entity.HasOne(d => d.Account)
            //    //    .WithMany(p => p.Dispositions)
            //    //    .HasForeignKey(d => d.AccountId)
            //    //    .OnDelete(DeleteBehavior.Cascade)
            //    //    .HasConstraintName("FK_Dispositions_Accounts");

            //    //entity.HasOne(d => d.Customer)
            //    //    .WithMany(p => p.Dispositions)
            //    //    .HasForeignKey(d => d.CustomerId)
            //    //    .OnDelete(DeleteBehavior.Cascade)
            //    //    .HasConstraintName("FK_Dispositions_Customers");
            //});

            modelBuilder.Entity<Loan>(entity =>
            {
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

            modelBuilder.Entity<Transaction>(entity =>
            {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
