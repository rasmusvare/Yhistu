using System;
using System.Linq;
using System.Threading.Tasks;
using App.BLL;
using App.BLL.DTO;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.DAL.EF;
using App.DAL.EF.Migrations;
using App.DAL.EF.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using Testing.WebApp.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Testing.WebApp.UnitTests;

public class MeterReadingServiceTests
{
    private readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder;
    private TestDataHelper _dataHelper;


    public MeterReadingServiceTests(ITestOutputHelper testOutputHelper)
    {
        // Set up Db context for testing using InMemoryDb
        _optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        _optionsBuilder.EnableSensitiveDataLogging();

        // Create new INMemory database instance with random name, so that parallel test methods do not conflict
        _optionsBuilder.UseInMemoryDatabase(new Guid().ToString());


        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<IMeterReadingService>();
    }

    public async Task<IAppBLL> GetBLL()
    {
        var context = new AppDbContext(_optionsBuilder.Options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<App.DAL.EF.AutoMapperConfig>();
                cfg.AddProfile<App.BLL.AutoMapperConfig>();
            }
        );
        var mapper = mockMapper.CreateMapper();
        var uow = new AppUOW(context, mapper);
        var appBLL = new AppBLL(uow, mapper);

        _dataHelper = new TestDataHelper(context, appBLL);
        await _dataHelper.SeedData();

        return appBLL;
    }


    // Test if data helper works correctly

    [Fact]
    public async Task Test_GetAll_Associations_Returns_All()
    {
        var bll = await GetBLL();
        var associations = bll.Associations.GetAll().ToList();

        associations.Should().NotBeNull();
        associations.Should().NotBeEmpty();
        associations.Count.Should().Be(2);
        associations.First().Name.Should().Be("Test association");
    }


    [Fact]
    public async Task Test_Async_GetAll_Associations_Returns_All()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var associations = (await bll.Associations.GetAllAsync(personId)).ToList();

        associations.Should().NotBeNull();
        associations.Should().NotBeEmpty();
        associations.Count.Should().Be(2);
        associations.First().Name.Should().Be("Test association");
    }

    [Fact]
    public async Task Test_Async_GetAll_Apartments_Returns_All()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var apartments = (await bll.Apartments.GetAllAsync(personId)).ToList();

        apartments.Should().NotBeNull();
        apartments.Should().NotBeEmpty();
        apartments.Count.Should().Be(3);
        apartments.First().AptNumber.Should().Be("1");
    }

    // ============ Meters and MeterReadings test start ============
    // Async methods

    [Fact]
    public async Task Test_Async_GetAll_Meters_Returns_One()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meters = (await bll.Meters.GetAllAsync(apartment.Id, "apartment")).ToList();

        meters.Should().NotBeNull();
        meters.Should().NotBeEmpty();
        meters.Count.Should().Be(1);
    }

    [Fact]
    public async Task Test_Async_GetAll_MeterReadings_Returns_Empty()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var meterReadings = await bll.MeterReadings.GetAllAsync(meter.Id);

        meterReadings.Should().NotBeNull();
        meterReadings.Should().BeEmpty();
    }

    [Fact]
    public async Task Test_Async_MeterReading_Add()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        bll.MeterReadings.Add(new MeterReading()
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        });

        await bll.SaveChangesAsync();

        var meterReadings = await bll.MeterReadings.GetAllAsync(meter.Id);

        meterReadings.Should().NotBeNull();
        meterReadings.Should().NotBeEmpty();
        meterReadings.First().Value.Should().Be(100);
    }

    
    [Fact]
    public async Task Test_Async_MeterReading_RemoveById()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        await bll.MeterReadings.RemoveAsync(reading.Id);
        await bll.SaveChangesAsync();

        var meterReadings = await bll.MeterReadings.GetAllAsync(meter.Id);

        meterReadings.Should().NotBeNull();
        meterReadings.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Test_Async_MeterReadings_RemoveById_NotExistingKey()
    {
        var bll = await GetBLL();

        await Assert.ThrowsAsync<NullReferenceException>(()=> bll.MeterReadings.RemoveAsync(new Guid()));
    }

    
    [Fact]
    public async Task Test_Async_MeterReadings_Get()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        var readingFromDb = await bll.MeterReadings.FirstOrDefaultAsync(reading.Id);

        readingFromDb.Should().NotBeNull();
        readingFromDb.Value.Should().Be(100);
    }

    [Fact]
    public async Task Test_Async_MeterReadings_Update()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        reading.Value = 250;

        bll.MeterReadings.Update(reading);
        await bll.SaveChangesAsync();

        var updated = bll.MeterReadings.GetUpdatedEntityAfterSaveChanges(reading);

        var meterReadings = (await bll.MeterReadings.GetAllAsync(meter.Id)).ToList();

        meterReadings.Should().NotBeNull();
        meterReadings.Should().NotBeEmpty();
        meterReadings.Count.Should().Be(1);
        updated.Value.Should().Be(250);
        meterReadings.First().Value.Should().Be(250);
    }

    [Fact]
    public async Task Test_Async_MeterReadings_Update2()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };


        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        _dataHelper.ClearEntityCache();

        var readingUpdate = new MeterReading
        {
            Id = reading.Id,
            MeterId = meter.Id,
            Value = 250,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Update(readingUpdate);
        await bll.SaveChangesAsync();

        var meterReadings = (await bll.MeterReadings.GetAllAsync(meter.Id)).ToList();

        meterReadings.Should().NotBeNull();
        meterReadings.Should().NotBeEmpty();
        meterReadings.Count.Should().Be(1);
        meterReadings.First().Value.Should().Be(250);
    }

    [Fact]
    public async Task Test_Async_MeterReadings_Exists()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        var exists = await bll.MeterReadings.ExistsAsync(reading.Id);
        exists.Should().BeTrue();
    }


    // Sync methods


    [Fact]
    public async Task Test_MeterReadings_Exists()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        var exists = bll.MeterReadings.Exists(reading.Id);
        exists.Should().BeTrue();
    }
    
    [Fact]
    public async Task Test_MeterReadings_Get()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        bll.SaveChanges();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        var readingFromDb = bll.MeterReadings.FirstOrDefault(reading.Id);

        readingFromDb.Should().NotBeNull();
        readingFromDb.Value.Should().Be(100);
    }
    
    [Fact]
    public async Task Test_MeterReading_Remove()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        bll.SaveChanges();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        bll.MeterReadings.Remove(reading);
        bll.SaveChanges();

        var meterReadings = await bll.MeterReadings.GetAllAsync(meter.Id);

        meterReadings.Should().NotBeNull();
        meterReadings.Should().BeEmpty();
    }


    [Fact]
    public async Task Test_MeterReadings_RemoveById()
    {
        var bll = await GetBLL();
        var personId = await _dataHelper.GetAdminPersonId();
        var association = (await bll.Associations.GetAllAsync(personId)).First();
        var apartment = (await bll.Apartments.GetAllAsync(personId)).First();

        var meter = (await bll.Meters.GetAllAsync(apartment.Id, "apartment"))!.First();

        var reading = new MeterReading
        {
            MeterId = meter.Id,
            Value = 100,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };

        bll.MeterReadings.Add(reading);

        await bll.SaveChangesAsync();
        reading.Id = bll.MeterReadings.GetIdFromDb(reading);

        bll.MeterReadings.Remove(reading.Id);
        await bll.SaveChangesAsync();

        var meterReadings = await bll.MeterReadings.GetAllAsync(meter.Id);

        meterReadings.Should().NotBeNull();
        meterReadings.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Test_MeterReadings_RemoveById_NotExistingKey()
    {
        var bll = await GetBLL();

        Assert.Throws<NullReferenceException>(()=>bll.MeterReadings.Remove(new Guid()));
    }

}