using BookStore.Application.Interfaces;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;


namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            
            }
               );

        services.AddScoped<IBookStoreContext>(provider => provider.GetRequiredService<BookStoreDbContext>());
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
       
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}