using System;
using System.IO;
using System.Reflection;
using Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        string? envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot? configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", true)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{envName}.json", true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .Build();

        DbContextOptionsBuilder<IdentityContext> builder = new DbContextOptionsBuilder<IdentityContext>();
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        ;
        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
 

        return new IdentityContext(builder.Options);
    }
}