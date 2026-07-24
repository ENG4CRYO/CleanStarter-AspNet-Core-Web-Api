using CleanStarter.Core.Entities;
using CleanStarter.Core.Entities.AuthEntites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Application.Interfaces.Common
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
