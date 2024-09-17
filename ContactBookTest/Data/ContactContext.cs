using ContactBookTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactBookTest.Data;

public class ContactContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "6247";
        optionsBuilder.UseNpgsql($"Host={host};Port={port};Database=ContactBookDb;Username={user};Password={pass}");
    }
}