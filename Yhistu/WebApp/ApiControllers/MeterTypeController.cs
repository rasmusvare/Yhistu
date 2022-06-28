#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MeterTypeController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MeterTypeMapper _meterTypeMapper;

    public MeterTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _meterTypeMapper = new MeterTypeMapper(mapper);
    }

    // GET: api/MeterType

    /// <summary>
    /// Returns all the meter types of the association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of meter types</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.MeterType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.MeterType>>> GetMeterTypes(Guid associationId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Associations.ExistsAsync(associationId))
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
                        $"Association with id {associationId} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }
        
        if (!await _bll.Members.IsMember(personId, associationId))
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
                        "User does not have permission to view the meter types of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok((await _bll.MeterTypes.GetAllAsync(associationId)).Select(x => _meterTypeMapper.Map(x)).ToList());
    }

    // GET: api/MeterTypedetails//5

    /// <summary>
    /// Gets the details of a particular meter type
    /// </summary>
    /// <param name="id">Id of the meter type</param>
    /// <returns>Meter Type object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeterType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeterType>> GetMeterType(Guid id)
    {
        var meterType = await _bll.MeterTypes.FirstOrDefaultAsync(id);

        if (meterType?.AssociationId == null)
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
                        "Meter type not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, meterType.AssociationId.Value))
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
                        "User does not have permission to view this meter type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_meterTypeMapper.Map(meterType));
    }


    // PUT: api/MeterType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified meter type 
    /// </summary>
    /// <param name="id">Id of the meter type</param>
    /// <param name="meterType">Supply updated data for the meter type</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMeterType(Guid id, MeterType meterType)
    {
        if (id != meterType.Id)
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

        var meterTypeDb = await _bll.MeterTypes.FirstOrDefaultAsync(id);

        if (meterTypeDb?.AssociationId == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
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

        if (!await _bll.Members.IsAdmin(personId, meterTypeDb.AssociationId.Value))
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

        _bll.MeterTypes.Update(_meterTypeMapper.Map(meterType)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    
    // POST: api/MeterType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new meter type
    /// </summary>
    /// <param name="meterType">Supply data for the new meter type</param>
    /// <returns>The newly created meter type object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeterType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MeterType>> PostMeterType(MeterType meterType)
    {
        if (meterType.AssociationId == null)
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
                        "User does not have permission to add a new global meter type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }
        
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (meterType.AssociationId != null &&
            !await _bll.Members.IsAdmin(personId, meterType.AssociationId.Value))
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

        var bllEntity = _meterTypeMapper.Map(meterType)!;
        _bll.MeterTypes.Add(bllEntity);
        await _bll.SaveChangesAsync();
        meterType.Id = _bll.MeterTypes.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMeterType", new {id = meterType.Id}, meterType);
    }

    // DELETE: api/MeterType/5

    /// <summary>
    /// Deletes the specified meter type
    /// </summary>
    /// <param name="id">Id of the meter type</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMeterType(Guid id)
    {
        RestErrorResponse errorResponse;

        var meterType = await _bll.MeterTypes.FirstOrDefaultAsync(id);

        if (meterType?.AssociationId == null)
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
                        $"Meter type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (await _bll.Members.IsAdmin(personId, meterType.AssociationId.Value))
        {
            if (meterType.Meters == null || meterType.Meters.Count == 0)
            {
                _bll.MeterTypes.Remove(meterType);
                await _bll.SaveChangesAsync();
                return NoContent();
            }
            
            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorised"] = new List<string>
                    {
                        "Please remove this meter type from all meters before deleting"
                    }
                }
            };
            return BadRequest(errorResponse);
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

    private async Task<bool> MeterTypeExists(Guid id)
    {
        return await _bll.MeterTypes.ExistsAsync(id);
    }
}