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
public class ContactTypeController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ContactTypeMapper _contactTypeMapper;

    public ContactTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _contactTypeMapper = new ContactTypeMapper(mapper);
    }

    // GET: api/ContactType

    /// <summary>
    /// Returns all the global contact types and custom contact types of the association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of contact types</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ContactType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ContactType>>> GetContactTypes(Guid associationId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        RestErrorResponse errorResponse;
        if (!await _bll.Associations.ExistsAsync(associationId))
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
                        $"Association with id {associationId} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (!await _bll.Members.IsMember(personId, associationId))
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
                        "User does not have permission to view the contact types of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var contactTypes = await _bll.ContactTypes.GetAllAsync(associationId);
        return Ok(contactTypes.Select(x => _contactTypeMapper.Map(x)).ToList());
    }

    // GET: api/ContactType/5

    /// <summary>
    /// Gets the details of a particular contact type
    /// </summary>
    /// <param name="id">Id of the contact type</param>
    /// <returns>Contact type object</returns>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ContactType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactType>> GetContactType(Guid id)
    {
        var contactType = await _bll.ContactTypes.FirstOrDefaultAsync(id);

        if (contactType == null || contactType.AssociationId == null)
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
                        "Contact type not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, contactType.AssociationId.Value))
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
                        "User does not have permission to view this contact type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_contactTypeMapper.Map(contactType));
    }

    // PUT: api/ContactType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of a particular contact type
    /// </summary>
    /// <param name="id">Id of the contact type</param>
    /// <param name="contactType">Supply updated data for teh contact type</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutContactType(Guid id, ContactType contactType)
    {
        if (id != contactType.Id)
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

        var contactTypeDb = await _bll.ContactTypes.FirstOrDefaultAsync(id);

        if (contactTypeDb?.AssociationId == null)
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
                        $"Contact type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, contactTypeDb.AssociationId.Value))
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

        _bll.ContactTypes.Update(_contactTypeMapper.Map(contactType)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/ContactType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new contact type
    /// </summary>
    /// <param name="contactType">Supply data for the new contact type</param>
    /// <returns>The newly created contact type object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.ContactType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ContactType>> PostContactType(ContactType contactType)
    {
        if (contactType.AssociationId == null)
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

        if (contactType.AssociationId != null &&
            !await _bll.Members.IsAdmin(personId, contactType.AssociationId.Value))
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

        var bllEntity = _contactTypeMapper.Map(contactType)!;
        _bll.ContactTypes.Add(bllEntity);
        await _bll.SaveChangesAsync();
        contactType.Id = _bll.ContactTypes.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetContactType", new {id = contactType.Id}, contactType);
    }

    
    // DELETE: api/ContactType/5
    
    /// <summary>
    /// Deletes the specified contact type
    /// </summary>
    /// <param name="id">Id of the contact type</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContactType(Guid id)
    {
        RestErrorResponse errorResponse;

        var contactType = await _bll.ContactTypes.FirstOrDefaultAsync(id);

        if (contactType?.AssociationId == null)
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

        if (await _bll.Members.IsAdmin(personId, contactType.AssociationId.Value))
        {
            if (contactType.Contacts == null || contactType.Contacts.Count == 0)
            {
                _bll.ContactTypes.Remove(contactType);
                await _bll.SaveChangesAsync();
                return NoContent();
            }

            errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Unauthorised",
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

    private async Task<bool> ContactTypeExists(Guid id)
    {
        return await _bll.ContactTypes.ExistsAsync(id);
    }
    
}