namespace Meetekat.WebApi.Persistence;

using System.Reflection;
using Meetekat.WebApi.Entities.Meetups;
using Meetekat.WebApi.Entities.Users;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Meetup> Meetups => Set<Meetup>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Organizer> Organizers => Set<Organizer>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
