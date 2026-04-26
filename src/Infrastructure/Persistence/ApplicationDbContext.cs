using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentLoggedInUserService _currentLoggedInUserService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentLoggedInUserService currentLoggedInUserService)
            : base(options)
        {
            _currentLoggedInUserService = currentLoggedInUserService;
        }

        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<EventDetail> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notice> Notices { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var userId = _currentLoggedInUserService.UserId;

            foreach (var entry in ChangeTracker.Entries<CommonEntities>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = now.ToString();
                    entry.Entity.CreatedBy = userId.ToString();
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = now.ToString();
                    entry.Entity.UpdatedBy = userId.ToString();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}