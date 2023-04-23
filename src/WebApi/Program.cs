
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BookStore.Application;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Interfaces;
using FluentValidation.AspNetCore;
using Identity;
using Identity.Contexts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Persistence;
using Persistence.Seed;
using Services;
using WebApi.Filters;
using WebApi.Middlewares;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddApplication();
builder.Services.AddPersistanceIdentity(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSharedServices(builder.Configuration);

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityContext>();

builder.Services.AddControllersWithViews(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

builder.Services.AddRazorPages();

// Customise default API behaviour

builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();
builder.Services.AddTransient<RequestBodyLoggingMiddleware>();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddApplicationInsightsTelemetry(a =>
{
    a.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString");
});

builder.Services.AddSingleton<ITelemetryInitializer, ApplicationTelemetryInitializer>();
builder.Services.AddLogging(builder =>
{

    builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
    builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Debug);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();

app.UseCors(a => a.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<RequestBodyLoggingMiddleware>();
app.UseMiddleware<ResponseBodyLoggingMiddleware>();


app.MapControllers();
await Init();
await app.RunAsync();

async Task Init()
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider; 
            var context = services.GetRequiredService<BookStoreDbContext>();
            if (context.Database.IsRelational())
            {
                context.Database.Migrate();
            }
          

            if (app.Environment.IsDevelopment())
            {
                var _bookStoreContext = services.GetRequiredService<IBookStoreContext>();
                await Seed.Init(_bookStoreContext);
            }
               

        }
    }
    catch (Exception e)
    {
       

        throw new Exception("An error occurred while migrating or seeding the database.",e);
    }

}

public partial class Program { }