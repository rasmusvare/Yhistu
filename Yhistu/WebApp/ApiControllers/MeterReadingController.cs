#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using Microsoft.AspNetCore.Mvc;
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
public class MeterReadingController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MeterReadingMapper _meterReadingMapper;

    public MeterReadingController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _meterReadingMapper = new MeterReadingMapper(mapper);
    }

    // GET: api/MeterReading

    /// <summary>
    /// Returns all the readings of the specified meter
    /// </summary>
    /// <param name="meterId">Id of the meter</param>
    /// <returns>List of meter readings</returns>
    [HttpGet("{meterId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.MeterReading>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.MeterReading>>> GetMeterReadings(Guid meterId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meterId);

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
                        "Cannot check the association id. Contact support"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var meter = await _bll.Meters.FirstOrDefaultAsync(meterId);

        if (meter?.ApartmentId == null
            && meter?.BuildingId == null)
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
        
        if ( meter.ApartmentId != null &&
            !await _bll.ApartmentPersons.HasConnection(personId, meter.ApartmentId.Value) &&
            !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to view the meter readings of this meter"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }
        
        if ( meter.ApartmentId != null &&
            !await _bll.ApartmentPersons.HasConnection(personId, meter.ApartmentId.Value) &&
            !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to view the meter readings of this meter"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok((await _bll.MeterReadings.GetAllAsync(meterId)).Select(x => _meterReadingMapper.Map(x)));
    }

    // GET: api/MeterReading/5

    /// <summary>
    /// Gets the details of a particular meter reading 
    /// </summary>
    /// <param name="id">Id of hte metere reading</param>
    /// <returns>Meter reading object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeterReading), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeterReading>> GetMeterReading(Guid id)
    {
        var meterReading = await _bll.MeterReadings.FirstOrDefaultAsync(id);

        if (meterReading == null)
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
                        "Meter readings not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meterReading.MeterId);

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
                        "Cannot check the association id. Contact support"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (!await _bll.Members.IsMember(personId, associationId.Value))
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
                        "User does not have permission to view these meter readings"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_meterReadingMapper.Map(meterReading));
    }

    // PUT: api/MeterReading/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified meter reading
    /// </summary>
    /// <param name="id">Id of the meter</param>
    /// <param name="meterReading">Supply updated data for the meter reading</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMeterReading(Guid id, MeterReading meterReading)
    {
        if (id != meterReading.Id)
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

        var meterReadingDb = await _bll.MeterReadings.FirstOrDefaultAsync(id);

        if (meterReadingDb == null)
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
                        $"Meter reading with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meterReadingDb.Id);

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
                        "Cannot check the association id. Contact support"
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
                        "User does not have permission to update these meter readings"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.MeterReadings.Update(_meterReadingMapper.Map(meterReading)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }


    // POST: api/MeterReading
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
    /// <summary>
    /// Creates a new meter reading
    /// </summary>
    /// <param name="meterReading">Supply data for the meter reading</param>
    /// <returns>Newly created meter reading object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeterReading), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MeterReading>> PostMeterReading(MeterReading meterReading)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meterReading.MeterId);

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
                        "Cannot check the association id. Contact support"
                    }
                }
            };
            return BadRequest(errorResponse);
        }
        
        var meter = await _bll.Meters.FirstOrDefaultAsync(meterReading.MeterId);

        if (meter?.ApartmentId == null &&
            meter?.BuildingId==null)
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
        
        if (meter.ApartmentId != null &&
            !await _bll.ApartmentPersons.HasConnection(personId, meter.ApartmentId.Value) &&
            !await _bll.Members.IsAdmin(personId, associationId.Value))
        

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
                        "User does not have permission to add a new meter reading"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _meterReadingMapper.Map(meterReading)!;
        if (!await _bll.MeterReadings.Validate(bllEntity))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Reading error"] = new List<string>
                    {
                        "New reading cannot be smaller than the previous one."
                    }
                }
            };
            return BadRequest(errorResponse);
        }
        
        _bll.MeterReadings.Add(bllEntity);
        await _bll.SaveChangesAsync();
        meterReading.Id = _bll.MeterReadings.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMeterReading", new {id = meterReading.Id}, meterReading);
    }

    
    // DELETE: api/MeterReading/5
    
    /// <summary>
    /// Deletes the specified metere reading
    /// </summary>
    /// <param name="id">Id of the meter reading</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMeterReading(Guid id)
    {
        var meterReading = await _bll.MeterReadings.FirstOrDefaultAsync(id);

        RestErrorResponse errorResponse;
        if (meterReading == null)
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
                        $"Meter type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Meters.GetAssociationId(meterReading.MeterId);

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

            _bll.MeterReadings.Remove(meterReading);
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

    private async Task<bool> MeterReadingExists(Guid id)
    {
        return await _bll.MeterReadings.ExistsAsync(id);
    }
}