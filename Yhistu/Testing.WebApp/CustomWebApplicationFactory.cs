using System;
using System.Linq;
using App.BLL;
using App.DAL.EF;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testing.WebApp.Helpers;

namespace Testing.WebApp;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    private static bool dbInitialized = false;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(async services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // and new DbContext
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });
            
            // data seeding
            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

            db.Database.EnsureCreated();
            
            var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<App.DAL.EF.AutoMapperConfig>();
                    cfg.AddProfile<App.BLL.AutoMapperConfig>();
                }
            );
            var mapper = mockMapper.CreateMapper();
            var uow = new AppUOW(db, mapper);
            var appBLL = new AppBLL(uow, mapper);

            var dataHelper = new TestDataHelper(db, appBLL);

            try
            {
                if (dbInitialized == false)
                {
                    dbInitialized = true;
                    await dataHelper.SeedData();

                    // DataSeeder.SeedData(db);
                    // if (db.FooBars.Any()) return;
                    // db.FooBars.Add(new FooBar() {Value = "Testing - " + Guid.NewGuid().ToString()});
                    // db.SaveChanges();
                    
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        });
    }
}
