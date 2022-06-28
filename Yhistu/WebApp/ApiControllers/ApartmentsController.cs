#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ApartmentsController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ApartmentMapper _apartmentMapper;
    private readonly PersonMapper _personMapper;
    private readonly MeterMapper _meterMapper;
    
    public ApartmentsController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _apartmentMapper = new ApartmentMapper(mapper);
        _personMapper = new PersonMapper(mapper);
        _meterMapper = new MeterMapper(mapper);
    }


    // GET: api/Apartments

    /// <summary>
    /// Returns all the apartments associated with the user
    /// </summary>
    /// <returns>List of apartments</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Apartment>>> GetApartments()
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        var res = (await _bll.Apartments.GetAllAsync(personId))
            .Select(a => _apartmentMapper.Map(a)).ToList();

        return Ok(res);
    }

/*
    // GET: api/Apartments

    /// <summary>
    /// Returns all the apartments connected to the specified association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of apartments</returns>
    [HttpGet("association/{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Apartment>>> GetAssociationApartments(Guid associationId)
    {
        //TODO : !!!
        return Ok();
    }
*/

    // GET: api/Apartments

    /// <summary>
    /// Returns all the apartments connected to the specified building
    /// </summary>
    /// <param name="buildingId">Id of the building</param>
    /// <returns>List of apartments</returns>
    [HttpGet("{buildingId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), 200)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Apartment>>> GetBuildingApartments(Guid buildingId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var building = await _bll.Buildings.FirstOrDefaultAsync(buildingId);

        if (building == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Building with id {buildingId} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (!await _bll.Members.IsAdmin(personId, building.AssociationId))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "User does not have permission to view the apartments of this association"
                    }
                }
            };

            return Unauthorized(errorResponse);
        }

        var apartments = (await _bll.Apartments.GetAllInBuildingAsync(buildingId)).Select(x => _apartmentMapper.Map(x))
            .ToList();

        foreach (var each in apartments)
        {
            each.Persons = (await _bll.ApartmentPersons.GetAllApartmentAsync(each.Id))
                .Select(ap=> _personMapper.Map(ap.Person)).ToList();
            
            each.Meters = (await _bll.Meters.GetAllAsync(each.Id, "apartment"))?
                .Select(m=> _meterMapper.Map(m)).ToList();
        }

        // return (await _bll.Apartments.GetAllInBuildingAsync(buildingId)).Select(x => _apartmentMapper.Map(x)).ToList();

        return Ok(apartments);
    }


    // GET: api/Apartments/details/5

    /// <summary>
    /// Gets the details of the specified apartment
    /// </summary>
    /// <param name="id">id of the apartment</param>
    /// <returns>Apartment object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Apartment>> GetApartment(Guid id)
    {
        var apartmentDb = await _bll.Apartments.FirstOrDefaultAsync(id);

        if (apartmentDb == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Apartment with id {id} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(id);

        if (associationId == null
            || (!await _bll.Members.IsAdmin(personId, associationId.Value)
                && !await _bll.ApartmentPersons.HasConnection(personId, id)))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "User does not have permission to view this apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var apartment = _apartmentMapper.Map(apartmentDb)!;
        
        apartment.Persons = (await _bll.ApartmentPersons.GetAllApartmentAsync(apartment.Id))
            .Select(ap=> _personMapper.Map(ap.Person)).ToList();
            
        apartment.Meters = (await _bll.Meters.GetAllAsync(apartment.Id, "apartment"))?
            .Select(m=> _meterMapper.Map(m)).ToList();

        return Ok(apartment);
    }


    // PUT: api/Apartments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified apartment
    /// </summary>
    /// <param name="id">Id of the apartment</param>
    /// <param name="apartment">Supply updated data for the apartment</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutApartment(Guid id, Apartment apartment)
    {
        if (id != apartment.Id)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id mismatch"] = new List<string>
                    {
                        "Cannot change id on update"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (!await _bll.Apartments.ExistsAsync(id))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Apartment with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(id);

        if (associationId == null || !await _bll.Members.IsAdmin(personId, associationId.Value))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "User does not have permission to update this building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var building = await _bll.Buildings.FirstOrDefaultAsync(apartment.BuildingId);
        
        if (building == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Building with the id {apartment.BuildingId} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }
        

        var bllEntity = _apartmentMapper.Map(apartment)!;
        _bll.Apartments.Update(bllEntity);
        await _bll.SaveChangesAsync();
        await _bll.Buildings.CalculateAreas(building);
        await _bll.SaveChangesAsync();
        return Ok();
    }


    // POST: api/Apartments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new apartment in the building
    /// </summary>
    /// <param name="apartment">Supply details of the apartment</param>
    /// <returns>Newly created apartment</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Apartment), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Apartment>> PostApartment(Apartment apartment)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        var building = await _bll.Buildings.FirstOrDefaultAsync(apartment.BuildingId);
        
        if (building != null && !await _bll.Associations.ExistsAsync(building.AssociationId))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Association with the id {building.AssociationId} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (!await _bll.Members.IsAdmin(personId, building.AssociationId))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "User does not have permission to add a new apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }
        
        
        var bllEntity = _apartmentMapper.Map(apartment)!;
        _bll.Apartments.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Apartments.GetIdFromDb(bllEntity);

        await _bll.Buildings.CalculateAreas(building);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetApartment", new {id = bllEntity.Id}, bllEntity);
    }


    // DELETE: api/Apartments/5

    /// <summary>
    /// Deletes the specified apartment and all the meters and
    /// person-apartment connection associated with the apartment
    /// </summary>
    /// <param name="id">Id of the apartment</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteApartment(Guid id)
    {
        RestErrorResponse errorResponse;

        if (!await _bll.Apartments.ExistsAsync(id))
        {
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Apartment with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(id);

        if (associationId == null || !await _bll.Members.IsAdmin(personId, associationId.Value))
        {
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "User does not have permission to delete this apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var meters = await _bll.Meters.GetAllAsync(id, "apartment");
        if (meters != null)
        {
            foreach (var each in meters)
            {
                foreach (var reading in await _bll.MeterReadings.GetAllAsync(each.Id))
                {
                    _bll.MeterReadings.Remove(reading);
                }

                _bll.Meters.Remove(each);
            }
        }


        await _bll.SaveChangesAsync();

        var apartmentPersons = await _bll.ApartmentPersons.GetAllApartmentAsync(id);
        foreach (var person in apartmentPersons)
        {
            _bll.ApartmentPersons.Remove(person);
        }

        await _bll.SaveChangesAsync();

        var apartmentDb = await _bll.Apartments.FirstOrDefaultAsync(id);


        _bll.Apartments.Remove(apartmentDb!);
        await _bll.SaveChangesAsync();

        return NoContent();
    }


    private async Task<bool> ApartmentExists(Guid id)
    {
        return await _bll.Apartments.ExistsAsync(id);
    }
}