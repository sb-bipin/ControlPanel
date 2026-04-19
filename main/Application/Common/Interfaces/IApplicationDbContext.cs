using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<EventDetail> Events { get; set; }
        public DbSet<News> News { get; set; }

        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
