using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.DAL.DTO.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using DateOnlyTimeOnly.AspNet.Converters;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;
// using Association = App.Public.DTO.v1.Association;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Testing.WebApp.ApiControllers;

public class MeterReadingControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly JsonSerializerOptions _jsonOptions;
    private Association? _association;
    private Building? _building;
    private Apartment? _apartment;
    private Meter? _meter;

    public MeterReadingControllerTests(CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );

        _jsonOptions = new JsonSerializerOptions
        {
            Converters = {new DateOnlyJsonConverter()},
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task API_Main_HappyFlow()
    {
        // Arrange
        const string uri = "/api/v1/associations/";

        // Act
        var response = await _client.GetAsync(uri);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        await API_Register();
    }

    // [Fact, Priority(10)]
    public async Task API_Register()
    {
        // Arrange
        const string uri = "/api/v1/identity/account/register/";

        var registerDto = new Register
        {
            Email = "test@itcollege.ee",
            FirstName = "Rasmus",
            LastName = "Vare",
            IdCode = "38902180330",
            Password = "Kala.maja1"
        };

        var jsonStr = System.Text.Json.JsonSerializer.Serialize(registerDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(uri, data);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            return;
        }

        var requestContent = await response.Content.ReadAsStringAsync();

        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(
            requestContent, _jsonOptions
        );

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt.Token);

        await API_GetAssociations();
    }

    // [Fact, Priority(20)]
    public async Task API_GetAssociations()
    {
        // Arrange
        const string uri = "/api/v1/associations";

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
        }


        var responseData = JsonSerializer.Deserialize<IEnumerable<Association>>(
            responseBody, _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.Should().BeEmpty();

        await API_CreateAssociation();
    }

    // [Fact, Priority(4)]
    public async Task API_CreateAssociation()
    {
        // Arrange
        const string uri = "/api/v1/associations";

        var association = new Association()
        {
            Address = "Raja tn 4c",
            FoundedOn = DateOnly.FromDateTime(DateTime.Now),
            Name = "IT Kolledži korteriühistu",
            RegistrationNo = "123456789"
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(association, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(uri, data);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
            
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("Association: " + responseBody);

        var responseData = JsonSerializer.Deserialize<Association>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.Name.Should().Be("IT Kolledži korteriühistu");

        _association = responseData;

        await API_CreateBuilding();
    }

    // [Fact]
    public async Task API_CreateBuilding()
    {
        // Arrange
        const string uri = "/api/v1/buildings";

        var building = new Building
        {
            AssociationId = _association.Id,
            Address = "Raja tn 4c",
            Name = "IT Kolledži peahoone",
            CommonSqM = 2000
        };

        var jsonStr = System.Text.Json.JsonSerializer.Serialize(building, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(uri, data);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("Building: " + responseBody);

        var responseData = JsonSerializer.Deserialize<Building>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData!.Name.Should().Be("IT Kolledži peahoone");
        _building = responseData;

        await API_CreateApartment();
    }

    // [Fact]
    public async Task API_CreateApartment()
    {
        // Arrange
        const string uri = "/api/v1/apartments";

        var building = new Apartment
        {
            AptNumber = "1",
            TotalSqMtr = 100,
            NoOfRooms = 5,
            IsBusiness = false,
            BuildingId = _building!.Id
        };

        var jsonStr = System.Text.Json.JsonSerializer.Serialize(building, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(uri, data);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
            
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("Apartment: " + responseBody);

        var responseData = JsonSerializer.Deserialize<Apartment>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData!.AptNumber.Should().Be("1");
        responseData.TotalSqMtr.Should().Be(100);

        _apartment = responseData;

        await API_BuildingInfoUpdated();
    }

    [Fact]
    public async Task API_BuildingInfoUpdated()
    {
        // Arrange
        var uri = "/api/v1/buildings/" + _association!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = (await response.Content.ReadAsStringAsync());

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("Building updated: " + responseBody);

        var responseData = JsonSerializer.Deserialize<IEnumerable<Building>>(
            responseBody,
            _jsonOptions
        )!.First();

        responseData.Should().NotBeNull();
        responseData.NoOfApartments.Should().Be(1);
        responseData.ApartmentSqM.Should().Be(100);
        responseData.TotalSqM.Should().Be(2100);

        await API_AddPersonToApartment();
    }

    // [Fact]
    public async Task API_AddPersonToApartment()
    {
        // Arrange
        var uri = "/api/v1/person";
        var response = await _client.GetAsync(uri);

        var responseBody = await response.Content.ReadAsStringAsync();

        var person = JsonSerializer.Deserialize<IEnumerable<Person>>(
            responseBody,
            _jsonOptions
        )!.First();

        uri = "/api/v1/apartmentperson";

        var building = new ApartmentPerson
        {
            ApartmentId = _apartment!.Id,
            PersonId = person.Id,
            IsOwner = true,
            From = DateOnly.FromDateTime(DateTime.Now)
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(building, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");


        // Act
        response = await _client.PostAsync(uri, data);
        responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("ApartmentPerson: " + responseBody);

        var responseData = JsonSerializer.Deserialize<ApartmentPerson>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.PersonId.Should().Be(person.Id);
        responseData.ApartmentId.Should().Be(_apartment.Id);

        await API_GetApartmentPersons();
    }

    // [Fact]
    public async Task API_GetApartmentPersons()
    {
        // Arrange
        var uri = "/api/v1/apartmentperson/" + _apartment!.Id;

        // Act
        var response = await _client.GetAsync(uri);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
        }

        var responseBody = (await response.Content.ReadAsStringAsync());
        
        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("ApartmentPersons: " + responseBody);


        var responseData = JsonSerializer.Deserialize<IEnumerable<ApartmentPerson>>(
            responseBody,
            _jsonOptions
        )!.ToList();

        responseData.Should().NotBeNull();
        responseData.Count.Should().Be(1);
        responseData[0].ApartmentId.Should().Be(_apartment.Id);

        await API_GetMetersReturnsEmpty();
    }

    // [Fact]
    public async Task API_GetMetersReturnsEmpty()
    {
        // Arrange
        var uri = "/api/v1/meter/" + _apartment!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = (await response.Content.ReadAsStringAsync());

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        var responseData = JsonSerializer.Deserialize<IEnumerable<Meter>>(
            responseBody,
            _jsonOptions
        )!.ToList();

        responseData.Should().NotBeNull();
        responseData.Should().BeEmpty();

        await API_AddMeterToApartment();
    }

    // [Fact]
    public async Task API_AddMeterToApartment()
    {
        // Arrange
        var uri = "/api/v1/metertype/" + _association.Id;
        var response = await _client.GetAsync(uri);

        var responseBody = await response.Content.ReadAsStringAsync();

        var meterType = JsonSerializer.Deserialize<IEnumerable<MeterType>>(
            responseBody,
            _jsonOptions
        )!.First();

        uri = "/api/v1/meter";

        var meter = new Meter
        {
            ApartmentId = _apartment!.Id,
            BuildingId = _building!.Id,
            MeterTypeId = meterType.Id,
            MeterNumber = "Meeter_1",
            InstalledOn = DateOnly.FromDateTime(DateTime.Now)
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(meter, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");


        // Act
        response = await _client.PostAsync(uri, data);
        responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("Meter: " + responseBody);

        var responseData = JsonSerializer.Deserialize<Meter>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.ApartmentId.Should().Be(_apartment.Id);
        responseData.MeterNumber.Should().Be("Meeter_1");
        responseData.MeterReadings.Should().BeNull();

        _meter = responseData;

        await API_GetMeterReadingsReturnsEmpty();
    }

    // [Fact]
    public async Task API_GetMeterReadingsReturnsEmpty()
    {
        // Arrange
        var uri = "/api/v1/meterreading/" + _meter!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        var responseData = JsonSerializer.Deserialize<IEnumerable<MeterReading>>(
            responseBody,
            _jsonOptions
        )!.ToList();

        responseData.Should().NotBeNull();
        responseData.Should().BeEmpty();

        await API_AddMeterReading();
    }

    // [Fact]
    public async Task API_AddMeterReading()
    {
        const string uri = "/api/v1/meterreading";

        var meterReading = new MeterReading
        {
            MeterId = _meter!.Id,
            Value = 10,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(meterReading, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");


        // Act
        var response = await _client.PostAsync(uri, data);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine("ERROR:");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("MeterReading: " + responseBody);


        var responseData = JsonSerializer.Deserialize<MeterReading>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.Value.Should().Be(10);

        await API_GetMeterReadingsReturnsOne();
    }


    // [Fact]
    public async Task API_GetMeterReadingsReturnsOne()
    {
        // Arrange
        var uri = "/api/v1/meterreading/" + _meter!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();


        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine("ERROR: ");
            _testOutputHelper.WriteLine(responseBody);
        }

        var responseData = JsonSerializer.Deserialize<IEnumerable<MeterReading>>(
            responseBody,
            _jsonOptions
        )!.ToList();

        responseData.Should().NotBeNull();
        responseData.Count.Should().Be(1);
        responseData[0].Value.Should().Be(10);

        await API_Add_MeterReading2();
    }

    [Fact]
    public async Task API_Add_MeterReading2()
    {
        const string uri = "/api/v1/meterreading";

        var meterReading = new MeterReading
        {
            MeterId = _meter!.Id,
            Value = 20,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(meterReading, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");


        // Act
        var response = await _client.PostAsync(uri, data);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine(responseBody);
            return;
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine("MeterReading: " + responseBody);


        var responseData = JsonSerializer.Deserialize<MeterReading>(
            responseBody,
            _jsonOptions
        );

        responseData.Should().NotBeNull();
        responseData.Value.Should().Be(20);

        await API_GetAll_MeterReadings_Returns_Correct_Order();
    }

    // [Fact]
    public async Task API_GetAll_MeterReadings_Returns_Correct_Order()
    {
        // Arrange
        var uri = "/api/v1/meterreading/" + _meter!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine("");
            _testOutputHelper.WriteLine("ERROR: ");
            _testOutputHelper.WriteLine(responseBody);
        }

        var responseData = JsonSerializer.Deserialize<IEnumerable<MeterReading>>(
            responseBody,
            _jsonOptions
        )!.ToList();

        responseData.Should().NotBeNull();
        responseData.Count.Should().Be(2);
        responseData[0].Value.Should().Be(20);
        responseData[1].Value.Should().Be(10);

        await API_Add_MeterReading_Cannot_Add_Smaller();
    }

    // [Fact]

    public async Task API_Add_MeterReading_Cannot_Add_Smaller()
    {
        const string uri = "/api/v1/meterreading";

        var meterReading = new MeterReading
        {
            MeterId = _meter!.Id,
            Value = 5,
            Date = DateOnly.FromDateTime(DateTime.Now),
            AutoGenerated = false
        };
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(meterReading, _jsonOptions);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");


        // Act
        var response = await _client.PostAsync(uri, data);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await API_Login();
    }

    public async Task API_Login()
    {
        const string uri = "/api/v1/identity/account/login/";

        var loginDto = new Login()
        {
            Email = "admin@itcollege.ee",
            Password = "Kala.maja1"
        };

        var jsonStr = System.Text.Json.JsonSerializer.Serialize(loginDto);
        var data = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(uri, data);

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            return;
        }

        var requestContent = await response.Content.ReadAsStringAsync();

        var resultJwt = System.Text.Json.JsonSerializer.Deserialize<JwtResponse>(
            requestContent, _jsonOptions
        );

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resultJwt.Token);

        await API_GetAll_MeterReading_Stranger_Cannot_View();
    }


    public async Task API_GetAll_MeterReading_Stranger_Cannot_View()
    {
        // Arrange
        var uri = "/api/v1/meterreading/" + _meter!.Id;

        // Act
        var response = await _client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}