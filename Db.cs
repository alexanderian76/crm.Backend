using System.Numerics;
using System.Reflection;
using crm.Backend.Configuration;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class CrmDbContext : DbContext
{
    public DbSet<crm.Backend.Entities.Task> Tasks { get; set; }

    public CrmDbContext(DbContextOptions<CrmDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}