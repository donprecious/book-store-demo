using AutoMapper;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Common.Mappings;
using BookStore.Application.Interfaces;
using BookStore.Application.Library;
using BookStore.Domain.Events;
using BookStore.Domain.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Persistence;
using Persistence.Repositories;
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
                
            },  ServiceLifetime.Scoped);
            services.AddScoped<IBookStoreContext>(provider => provider.GetRequiredService<BookStoreDbContext>());
        
           
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
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

            
            }
        });  
    }
    
 
 
}