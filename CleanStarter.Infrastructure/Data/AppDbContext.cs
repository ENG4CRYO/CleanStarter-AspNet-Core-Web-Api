using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CleanStarter.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CleanStarter.Core.Entities.AuthEntites;

namespace CleanStarter.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        }
}
