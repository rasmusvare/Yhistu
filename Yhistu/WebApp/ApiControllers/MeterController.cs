#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using Microsoft.AspNetCore.Mvc;
// using App.DAL.EF;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MeterController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MeterMapper _meterMapper;
    private readonly MeterReadingMapper _meterReadingMapper;

    public MeterController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _meterMapper = new MeterMapper(mapper);
        _meterReadingMapper = new MeterReadingMapper(mapper);
    }

    // GET: api/Meter

    /// <summary>
    /// Returns all the meters connected to the specified apartment
    /// </summary>
    /// <param name="apartmentId">The Id of the apartment</param>
    /// <returns>List of meters</returns>
    [HttpGet("{apartmentId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Meter>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Meter>>> GetMeters(Guid apartmentId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var apartment = await _bll.Apartments.FirstOrDefaultAsync(apartmentId);


        RestErrorResponse errorResponse;
        if (apartment?.Building == null)
        {
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Apartment with id {apartmentId} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var associationId = apartment.Building.AssociationId;

        if (apartment.ApartmentPersons!.Any(a => a.PersonId == personId) ||
            await _bll.Members.IsAdmin(personId, associationId))
        {
            var meters = (await _bll.Meters.GetAllAsync(apartmentId, "apartment"))?.Select(x => _meterMapper.Map(x)!)
                .ToList();

            if (meters == null)
            {
                errorResponse = new RestErrorResponse
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                    Title = "Not found",
                    Status = HttpStatusCode.BadRequest,
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Errors =
                    {
                        ["Not found"] = new List<string>
                        {
                            $"No meters connected to the apartment number {apartment.AptNumber} found"
                        }
                    }
                };
                return NotFound(errorResponse);
            }
            // var meters = (await _bll.Meters.GetAllAsync(buildingId, "building"))?.Select(x => _meterMapper.Map(x)!)
            //     .ToList();

            foreach (var each in meters)
            {
                each.MeterReadings = (await _bll.MeterReadings.GetAllAsync(each.Id)).Select(m => _meterReadingMapper.Map(m))
                    .ToList();
            }

            return Ok(meters.Select(x => _meterMapper.Map(x)));
        }

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
                    "User does not have permission to view the meter types of this association"
                }
            }
        };
        return Unauthorized(errorResponse);

        // var buildingAssociationId = apartment.Building.AssociationId;
        //
        // return (await _bll.Meters.GetAllAsync(apartmentId, "apartment")).Select(x => _meterMapper.Map(x)!).ToList();
    }

    // GET: api/Meter

    /// <summary>
    /// Returns all the meters connected to the specified building only, 
    /// not including any meters of the apartments
    /// </summary>
    /// <param name="buildingId">Id of the building</param>
    /// <returns>List of meters</returns>
    [HttpGet("building/{buildingId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Meter>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Meter>>> GetOnlyBuildingMeters(Guid buildingId)
    {
        var building = await _bll.Buildings.FirstOrDefaultAsync(buildingId);

        if (building == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "Building not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

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
                        "User does not have permission to view the meters of the building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var meters = (await _bll.Meters.GetAllAsync(buildingId, "building"))?.Select(x => _meterMapper.Map(x)!)
            .ToList();

        foreach (var each in meters)
        {
            each.MeterReadings = (await _bll.MeterReadings.GetAllAsync(each.Id)).Select(m => _meterReadingMapper.Map(m))
                .ToList();
        }

        return Ok(meters);
    }

    // GET: api/Meter
    /// <summary>
    /// Returns all the meters connected to the specified building,
    /// including all the meters of the building's apartments
    /// </summary>
    /// <param name="buildingId">Id of the building</param>
    /// <returns>List of meters</returns>
    [HttpGet("all/{buildingId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Meter>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Meter>>> GetBuildingMeters(Guid buildingId)
    {
        var building = await _bll.Buildings.FirstOrDefaultAsync(buildingId);

        if (building == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "Building not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

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
                        "User does not have permission to view the meters of the building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var meters = (await _bll.Meters.GetAllAsync(buildingId, "all"))!.Select(x => _meterMapper.Map(x)!).ToList();

        foreach (var each in meters)
        {
            each.MeterReadings = (await _bll.MeterReadings.GetAllAsync(each.Id)).Select(m => _meterReadingMapper.Map(m))
                .ToList();
        }


        return Ok(meters);
    }

    // GET: api/Meter/5

    /// <summary>
    /// Gets the details of a particular meter
    /// </summary>
    /// <param name="id">Id of the meter</param>
    /// <returns>Meter object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Meter>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Meter>> GetMeter(Guid id)
    {
        var meter = await _bll.Meters.FirstOrDefaultAsync(id);

        if (meter == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "Meter not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var associationId = await _bll.Meters.GetAssociationId(meter.Id);

        if (associationId == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "Meter not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to view the meters of the building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }
        
        meter.MeterReadings = (await _bll.MeterReadings.GetAllAsync(meter.Id)).ToList();

        return _meterMapper.Map(meter);
    }


    // PUT: api/Meter/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified meter
    /// </summary>
    /// <param name="id">Id of the meter</param>
    /// <param name="meter">Supply updated data for the meter</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMeter(Guid id, Meter meter)
    {
        if (id != meter.Id)
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

        var meterDb = await _bll.Meters.FirstOrDefaultAsync(id);

        if (meterDb == null)
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
                        $"Meter with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meter.Id);

        if (!await _bll.Members.IsAdmin(personId, associationId!.Value))
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
                        "User does not have permission to update this meter type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.Meters.Update(_meterMapper.Map(meter)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Meter
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new meter
    /// </summary>
    /// <param name="meter">Supply data for the new meter</param>
    /// <returns>The newly created meter object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Meter), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Meter>> PostMeter(Meter meter)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        // Guid? associationId = null;
        // if (meter.ApartmentId != null)
        // {
        //     associationId = await _bll.Apartments.GetAssociationId(meter.ApartmentId.Value);
        // }
        // else if (meter.BuildingId != null)
        // {
        var building = await _bll.Buildings.FirstOrDefaultAsync(meter.BuildingId);
        var associationId = building?.AssociationId;
        // }

        if (associationId == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id problem"] = new List<string>
                    {
                        "Cannot connect the meter to a building. Check the Id"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (!await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to add a new meter type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _meterMapper.Map(meter)!;
        _bll.Meters.Add(bllEntity);
        await _bll.SaveChangesAsync();
        meter.Id = _bll.Meters.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMeter", new {id = meter.Id}, meter);
    }

    // DELETE: api/Meter/5

    /// <summary>
    /// Deletes the specified meter and it's readings
    /// </summary>
    /// <param name="id">Id of the meter type</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMeter(Guid id)
    {
        RestErrorResponse errorResponse;

        var meter = await _bll.Meters.FirstOrDefaultAsync(id);

        if (meter == null)
        {
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Meter with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }


        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        var associationId = await _bll.Meters.GetAssociationId(meter.Id);

        if (associationId == null)
        {
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id problem"] = new List<string>
                    {
                        "Cannot check the association id. Contact support"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (await _bll.Members.IsAdmin(personId, associationId.Value))
        {
            var readings = await _bll.MeterReadings.GetAllAsync(id);

            foreach (var each in readings)
            {
                await _bll.MeterReadings.RemoveAsync(each.Id);
            }

            await _bll.SaveChangesAsync();

            await _bll.Meters.RemoveAsync(meter.Id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

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
                    "User does not have permission to delete this meter type"
                }
            }
        };
        return Unauthorized(errorResponse);
    }

    private async Task<bool> MeterExists(Guid id)
    {
        return await _bll.Meters.ExistsAsync(id);
    }
}