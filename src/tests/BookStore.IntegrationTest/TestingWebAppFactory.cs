using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Persistence;
using Persistence.Seed;

namespace BookStore.IntegrationTest;

public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices( async (services) =>
        {
            
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<BookStoreDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryBookstoreTest");
                
            });
            services.AddScoped<IBookStoreContext>(provider => provider.GetRequiredService<BookStoreDbContext>());
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>())
            {
                try
                {  
                   
                    appContext.Database.EnsureCreated();
                    var context = scope.ServiceProvider.GetRequiredService<IBookStoreContext>();
                    await Seed.Init(context);
                   
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        });  
    }
}