using CleanArchitectureProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureProject.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<Book> Books { get; }
    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
