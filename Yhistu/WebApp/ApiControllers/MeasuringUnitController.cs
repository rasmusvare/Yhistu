#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
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
public class MeasuringUnitController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MeasuringUnitMapper _measuringUnitMapper;

    public MeasuringUnitController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _measuringUnitMapper = new MeasuringUnitMapper(mapper);
    }

    // GET: api/MeasuringUnit

    /// <summary>
    /// Returns all measuring units, both global ones and also the
    /// measuring units connected to the association.
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of measuring units</returns>
    [HttpGet("{associationId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.MeasuringUnit>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.MeasuringUnit>>> GetMeasuringUnits(Guid associationId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

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
                        "User does not have permission to view the measuring units of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return (await _bll.MeasuringUnits.GetAllAsync(associationId)).Select(x => _measuringUnitMapper.Map(x)).ToList();
    }

    // GET: api/MeasuringUnit/5

    /// <summary>
    /// Gets the details of a particular measuring unit
    /// </summary>
    /// <param name="id">Id of the measuring unit</param>
    /// <returns>Measuring unit object</returns>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeasuringUnit), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MeasuringUnit>> GetMeasuringUnit(Guid id)
    {
        var measuringUnit = await _bll.MeasuringUnits.FirstOrDefaultAsync(id);

        if (measuringUnit == null)
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
                        "Measuring unit not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (measuringUnit.AssociationId != null &&
            !await _bll.Members.IsMember(personId, measuringUnit.AssociationId.Value))
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
                        "User does not have permission to view this measuring unit"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_measuringUnitMapper.Map(measuringUnit));
    }


    // PUT: api/MeasuringUnit/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified meassuring unit
    /// </summary>
    /// <param name="id">Id of the measuring unit</param>
    /// <param name="measuringUnit">Supply updated data for the measuring unit</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMeasuringUnit(Guid id, MeasuringUnit measuringUnit)
    {
        if (id != measuringUnit.Id)
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

        var measuringUnitDb = await _bll.MeasuringUnits.FirstOrDefaultAsync(id);

        if (measuringUnitDb?.AssociationId == null)
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
                        $"Measuring unit with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, measuringUnitDb.AssociationId.Value))
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
                        "User does not have permission to update this measuring unit"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.MeasuringUnits.Update(_measuringUnitMapper.Map(measuringUnit)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }


    // POST: api/MeasuringUnit
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new measuring unit
    /// </summary>
    /// <param name="measuringUnit">Supply data for the new measuring unit</param>
    /// <returns>The newly created measuring unit object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MeasuringUnit), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MeasuringUnit>> PostMeasuringUnit(MeasuringUnit measuringUnit)
    {
        if (measuringUnit.AssociationId == null)
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
                        "User does not have permission to add a new global measuring unit"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (measuringUnit.AssociationId != null &&
            !await _bll.Members.IsAdmin(personId, measuringUnit.AssociationId.Value))
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
                        "User does not have permission to add a new measuring unit"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _measuringUnitMapper.Map(measuringUnit)!;
        _bll.MeasuringUnits.Add(bllEntity);
        await _bll.SaveChangesAsync();
        measuringUnit.Id = _bll.MeasuringUnits.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMeasuringUnit", new {id = measuringUnit.Id}, measuringUnit);
    }


    // DELETE: api/MeasuringUnit/5

    /// <summary>
    /// Deletes the specified measuring unit
    /// </summary>
    /// <param name="id">Id of the measuring unit</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMeasuringUnit(Guid id)
    {
        RestErrorResponse errorResponse;

        var measuringUnit = await _bll.MeasuringUnits.FirstOrDefaultAsync(id);

        if (measuringUnit?.AssociationId == null)
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
                        $"Measuring unit with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (await _bll.Members.IsAdmin(personId, measuringUnit.AssociationId.Value))
        {
            if (measuringUnit.MeterTypes == null || measuringUnit.MeterTypes.Count == 0)
            {
                _bll.MeasuringUnits.Remove(measuringUnit);
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
                        "Please all meter types using this measuring unit before deleting"
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
}