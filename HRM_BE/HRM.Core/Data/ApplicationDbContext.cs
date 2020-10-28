using System;
using System.Collections.Generic;
using System.Text;
using HRM.Core.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRM.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<TeamUser> TeamUser { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<JwtToken> JwtToken { get; set; }
    }
}
