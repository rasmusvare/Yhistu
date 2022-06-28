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
public class ApartmentPersonController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ApartmentPersonMapper _apartmentPersonMapper;

    public ApartmentPersonController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _apartmentPersonMapper = new ApartmentPersonMapper(mapper);
    }


    // GET: api/ApartmentPerson

    /// <summary>
    /// Gets all the apartment-person connection of the user
    /// </summary>
    /// <returns>List of ApartmentPerson objects</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ApartmentPerson), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ApartmentPerson>>> GetApartmentPersons()
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var res = (await _bll.ApartmentPersons.GetAllAsync(personId)).Select(x => _apartmentPersonMapper.Map(x))
            .ToList();

        return Ok(res);
    }


    // GET: api/ApartmentPerson/5

    /// <summary>
    /// Gets all the apartment-person connections of the specified apartment
    /// </summary>
    /// <param name="apartmentId">Id of the apartment</param>
    /// <returns>List of ApartmentPerson objects</returns>
    [HttpGet("{apartmentId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ApartmentPerson), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ApartmentPerson>>> GetApartmentPersons(Guid apartmentId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        // var building = await _bll.Buildings.FirstOrDefaultAsync(apartmentId);

        if (!await _bll.Apartments.ExistsAsync(apartmentId))
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
                        $"Apartment with id {apartmentId} not found."
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var associationId = await _bll.Apartments.GetAssociationId(apartmentId);


        if (associationId == null
            || (!await _bll.Members.IsAdmin(personId, associationId.Value)
                && !await _bll.ApartmentPersons.HasConnection(personId, apartmentId)))
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
                        "User does not have permission to view the persons associated with this apartment"
                    }
                }
            };

            return Unauthorized(errorResponse);
        }

        return Ok((await _bll.ApartmentPersons.GetAllApartmentAsync(apartmentId))
            .Select(x => _apartmentPersonMapper.Map(x)).ToList());
    }


    // GET: api/ApartmentPerson/details/5

    /// <summary>
    /// Gets the details of a particular apartment-person connection
    /// </summary>
    /// <param name="id">Id of the apartment-person connection</param>
    /// <returns>ApartmentPerson object</returns>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ApartmentPerson), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApartmentPerson>> GetApartmentPerson(Guid id)
    {
        var apartmentPerson = await _bll.ApartmentPersons.FirstOrDefaultAsync(id);

        if (apartmentPerson == null)
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
                        "User does not have permission to view members associated with this apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_apartmentPersonMapper.Map(apartmentPerson));
    }


    // PUT: api/ApartmentPerson/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified apartment-person connection
    /// </summary>
    /// <param name="id"></param>
    /// <param name="apartmentPerson"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutApartmentPerson(Guid id, ApartmentPerson apartmentPerson)
    {
        if (id != apartmentPerson.Id)
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

        var apartmentPersonDb = await _bll.ApartmentPersons.FirstOrDefaultAsync(id);

        if (apartmentPersonDb == null)
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
                        $"Apartment with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(apartmentPersonDb.ApartmentId);

        if (associationId == null
            || (!await _bll.Members.IsAdmin(personId, associationId.Value)
                && !await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId)
                && await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId) &&
                !apartmentPerson.IsOwner))
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
                        "User does not have permission to change members associated with this apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        if (await _bll.Members.IsAdmin(personId, associationId.Value) || apartmentPersonDb.IsOwner)
        {
            _bll.ApartmentPersons.Update(_apartmentPersonMapper.Map(apartmentPerson)!);
            await _bll.SaveChangesAsync();

            return Ok();
        }

        return BadRequest("Unknown problem");
    }


    // POST: api/ApartmentPerson
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new apartment-person connection
    /// </summary>
    /// <param name="apartmentPerson">Supply details of the apartment-person connection</param>
    /// <returns>Newly created apartment-person connection object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ApartmentPerson), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Apartment>> PostApartmentPerson(ApartmentPerson apartmentPerson)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(apartmentPerson.ApartmentId);

        if (associationId == null
            || (!await _bll.Members.IsAdmin(personId, associationId.Value)
                && !await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId)
                && await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId) &&
                !apartmentPerson.IsOwner))
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
                        "User does not have permission to add members to this apartment"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _apartmentPersonMapper.Map(apartmentPerson)!;

        if (await _bll.ApartmentPersons.FirstOrDefaultAsync(bllEntity) != null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Connection exists"] = new List<string>
                    {
                        "Person is already connected to the apartment"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (await _bll.Members.IsAdmin(personId, associationId.Value) || apartmentPerson.IsOwner)
        {
            _bll.ApartmentPersons.Add(bllEntity);
            await _bll.SaveChangesAsync();
            apartmentPerson.Id = _bll.ApartmentPersons.GetIdFromDb(bllEntity);

            return CreatedAtAction("GetApartmentPerson", new {id = apartmentPerson.Id}, apartmentPerson);
        }

        return BadRequest("Unknown problem. Contact the developer");
    }


    // DELETE: api/ApartmentPerson/5

    /// <summary>
    /// Deletes the specified apartment-person connection
    /// </summary>
    /// <param name="id">Id of the apartment-person connection</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteApartmentPerson(Guid id)
    {
        RestErrorResponse errorResponse;

        var apartmentPerson = await _bll.ApartmentPersons.FirstOrDefaultAsync(id);

        if (apartmentPerson == null)
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
                        $"Apartment-person connection with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        var associationId = await _bll.Apartments.GetAssociationId(apartmentPerson.ApartmentId);

        if (associationId == null
            || (!await _bll.Members.IsAdmin(personId, associationId.Value)
                && !await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId)
                && await _bll.ApartmentPersons.HasConnection(personId, apartmentPerson.ApartmentId) &&
                !apartmentPerson.IsOwner))
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
                        "User does not have permission to delete this apartment-person connection"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        if (apartmentPerson.IsOwner)
        {
            _bll.ApartmentPersons.Remove(apartmentPerson);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        return BadRequest("Unknown problem. Contact the developer");
    }

    private async Task<bool> ApartmentPersonExists(Guid id)
    {
        return await _bll.ApartmentPersons.ExistsAsync(id);
    }
}