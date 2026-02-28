using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineTicketManagementSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<
        User,
        Role,
        int,
        IdentityUserClaim<int>,
        UserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(tm => tm.Sender)
                .WithMany(u => u.TicketMessages)
                .HasForeignKey(tm => tm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasColumnName("RoleName");
        }
    }
}
