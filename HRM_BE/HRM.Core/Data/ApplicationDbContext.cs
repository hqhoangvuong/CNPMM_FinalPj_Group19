using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM.Core.Models.Timesheets;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRM.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<AccountDomain> AccountDomains { get; set; }
        public DbSet<UserAccountDomain> UserAccountDomain { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<TimesheetTask> TimesheetTasks { get; set; }
        public DbSet<TaskHour> TaskHour { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var pb in builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal))
                .Select(p => builder.Entity(p.DeclaringEntityType.ClrType).Property(p.Name)))
            {
                pb.HasColumnType("decimal(18,6)");
            }

            builder.Entity<TeamUser>().HasKey(ut => new { ut.TeamId, ut.UserId });

            builder.Entity<UserAccountDomain>().HasKey(ut => new { ut.AccountDomainId, ut.UserId });

            builder.Entity<User>().HasMany(u => u.TeamUsers).WithOne().HasForeignKey(ut => ut.UserId);
            builder.Entity<User>().HasMany(u => u.UserAccountDomains).WithOne().HasForeignKey(ut => ut.UserId);
            builder.Entity<User>().HasMany(u => u.Jobs).WithOne().HasForeignKey(ut => ut.UserId);

            builder.Entity<Team>().HasMany(u => u.Users).WithOne(tu => tu.Team).HasForeignKey(ut => ut.TeamId);

            builder.Entity<AccountDomain>().HasMany(u => u.Users).WithOne(tu => tu.AccountDomain).HasForeignKey(ut => ut.AccountDomainId);

            builder.Entity<Timesheet>().HasMany(t => t.Tasks);
            builder.Entity<TimesheetTask>().HasMany(tk => tk.TaskHours);
            builder.Entity<TimesheetTask>().HasOne(tk => tk.AccountDomain);
            builder.Entity<TimesheetTask>().HasOne(tk => tk.Activity);
            
        }
    }
}
