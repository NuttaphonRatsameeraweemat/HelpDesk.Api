using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelpDesk.Data.Pocos
{
    public partial class TicketIssueContext : DbContext
    {
        public TicketIssueContext()
        {
        }

        public TicketIssueContext(DbContextOptions<TicketIssueContext> options)
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=139.180.130.44;Database=TicketIssue;Username=postgres;Password=Leader26.26");
            }
        }

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
