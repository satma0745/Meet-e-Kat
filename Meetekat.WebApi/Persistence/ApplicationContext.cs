namespace Meetekat.WebApi.Persistence;

using System;
using Meetekat.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Meetup> Meetups => Set<Meetup>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Meetup>()
            .Property(meetup => meetup.Tags)
            .HasConversion(
                tags => string.Join(';', tags),
                aggregate => aggregate.Split(';', StringSplitOptions.None));

        modelBuilder
            .Entity<RefreshToken>()
            .HasKey(refreshToken => refreshToken.TokenId);
    }
}
