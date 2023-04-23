using System.Reflection;
using BookStore.Application.Common.Behaviours;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.InMemoryEvent;
using BookStore.Application.Library;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
      
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient<ILibraryService, LibraryService>(); 
            services.AddTransient<IDomainEventService, InMemoryEventPublisher>();
            return services;
        }
    }
}