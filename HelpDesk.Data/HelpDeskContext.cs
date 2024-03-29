﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HelpDesk.Data.Pocos;

namespace HelpDesk.Data
{
    public partial class HelpDeskContext : DbContext
    {
        public HelpDeskContext()
        {
        }

        public HelpDeskContext(DbContextOptions<HelpDeskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<EmailTask> EmailTask { get; set; }
        public virtual DbSet<Password> Password { get; set; }
        public virtual DbSet<Priority> Priority { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketComment> TicketComment { get; set; }
        public virtual DbSet<TicketTransection> TicketTransection { get; set; }
        public virtual DbSet<ValueHelp> ValueHelp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyCode)
                    .HasName("Company_pkey");

                entity.Property(e => e.CompanyCode).ValueGeneratedNever();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("Customer_pkey");

                entity.Property(e => e.Email).ValueGeneratedNever();
            });

            modelBuilder.Entity<Password>(entity =>
            {
                entity.HasKey(e => new { e.Email, e.Password1 })
                    .HasName("Password_pkey");
            });

            modelBuilder.Entity<ValueHelp>(entity =>
            {
                entity.HasKey(e => new { e.ValueType, e.ValueKey })
                    .HasName("ValueHelp_pkey");
            });
        }
    }
}
