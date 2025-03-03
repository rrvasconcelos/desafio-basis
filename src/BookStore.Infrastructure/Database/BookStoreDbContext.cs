using BookStore.Application.Abstractions.Data;
using BookStore.Domain.Authors;
using BookStore.Domain.Catalog;
using BookStore.Domain.Publishing;
using BookStore.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Database;

public class BookStoreDbContext(DbContextOptions<BookStoreDbContext> options, IPublisher publisher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookSubject> BookSubjects { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<BookPrice> BookPrices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
    
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}