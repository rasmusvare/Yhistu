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
public class ContactController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ContactMapper _contactMapper;

    public ContactController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _contactMapper = new ContactMapper(mapper);
    }

    // GET: api/Contact
    /// <summary>
    /// Returns all contacts of the user
    /// </summary>
    /// <returns>List of contacts</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Contact>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Contact>>> GetContacts()
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        return Ok((await _bll.Contacts.GetAllPersonAsync(personId)).Select(x => _contactMapper.Map(x)));
    }


    // GET: api/Contact/building/5

    /// <summary>
    /// Returns all the contacts of a particular building
    /// </summary>
    /// <param name="buildingId">Id of the building</param>
    /// <returns></returns>
    [HttpGet("building/{buildingId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Contact>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetBuildingContact(Guid buildingId)
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
                        $"Building with id {buildingId} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, building.AssociationId))
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
                        "User does not have permission to view the contacts of this building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var contacts = (await _bll.Contacts.GetAllBuildingAsync(buildingId)).Select(x => _contactMapper.Map(x))
            .ToList();

        return Ok(contacts);
    }

    // GET: api/Contact/association/5

    /// <summary>
    /// Returns all the contacts of a particular association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns></returns>
    [HttpGet("association/{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Contact>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetAssociationContacts(Guid associationId)
    {
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
                        "User does not have permission to view the contacts of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var contacts = (await _bll.Contacts.GetAllAssociationAsync(associationId)).Select(x => _contactMapper.Map(x))
            .ToList();

        return Ok(contacts);
    }


    // GET: api/details/Contact/5

    /// <summary>
    /// Returns the details of the specified contact
    /// </summary>
    /// <param name="id">Id of the contact</param>
    /// <returns>List of contacts</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("person/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Contact), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Contact>>> GetContact(Guid id)
    {
        var mainPersonId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        var contact = await _bll.Contacts.FirstOrDefaultAsync(id);

        if (contact == null)
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

        Guid? associationId = null;
        if (contact.AssociationId != null)
        {
            associationId = contact.AssociationId.Value;
        }
        else if (contact.BuildingId != null)
        {
            var building = await _bll.Buildings.FirstOrDefaultAsync(contact.BuildingId.Value);
            associationId = building!.AssociationId;
        }

        if (associationId != null && !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to update this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var person = (await _bll.Persons.FirstOrDefaultAsync(contact.PersonId!.Value))!;

        if ((associationId == null && person.AppUserId != User.GetUserId())
            || (associationId == null && await _bll.ApartmentPersons.ShareApartment(personId, contact.PersonId.Value)))
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
                        "User does not have permission to view this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }
        else if (associationId == null)
        {
        }


        return Ok((await _bll.Contacts.GetAllPersonAsync(personId)).Select(x => _contactMapper.Map(x)));
    }


    // PUT: api/Contact/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified contact
    /// </summary>
    /// <param name="id">Id of the contact</param>
    /// <param name="contact">Supply updated data for the contact</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutContact(Guid id, Contact contact)
    {
        if (id != contact.Id)
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

        if (contact.AssociationId == null
            && contact.BuildingId == null
            && contact.PersonId == null
           )
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
                        "Cannot determine the entity to add the contact to"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        Guid? associationId = null;
        if (contact.AssociationId != null)
        {
            associationId = contact.AssociationId.Value;
        }
        else if (contact.BuildingId != null)
        {
            var building = await _bll.Buildings.FirstOrDefaultAsync(contact.BuildingId.Value);
            associationId = building!.AssociationId;
        }

        if (associationId != null && !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to update this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var person = (await _bll.Persons.FirstOrDefaultAsync(contact.PersonId!.Value))!;

        if (associationId == null && person.AppUserId != User.GetUserId())
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
                        "User does not have permission to update this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.Contacts.Update(_contactMapper.Map(contact)!);
        await _bll.SaveChangesAsync();

        return Ok();
    }


    // POST: api/Contact
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new contact
    /// </summary>
    /// <param name="contact">Supply data for the new contact</param>
    /// <returns>The newly created contact object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Contact), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> PostContact(Contact contact)
    {
        if (contact.AssociationId == null
            && contact.BuildingId == null
            && contact.PersonId == null
           )
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
                        "Cannot determine the entity to add the contact to"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        Guid? associationId = null;
        if (contact.AssociationId != null)
        {
            associationId = contact.AssociationId.Value;
        }
        else if (contact.BuildingId != null)
        {
            var building = await _bll.Buildings.FirstOrDefaultAsync(contact.BuildingId.Value);
            associationId = building!.AssociationId;
        }

        if (associationId != null && !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to update this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var person = (await _bll.Persons.FirstOrDefaultAsync(contact.PersonId!.Value))!;

        if (associationId == null && person.AppUserId != User.GetUserId())
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
                        "User does not have permission to update this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _contactMapper.Map(contact)!;
        _bll.Contacts.Add(bllEntity);
        await _bll.SaveChangesAsync();
        contact.Id = _bll.Contacts.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetContact", new {id = contact.Id}, contact);
    }


    // DELETE: api/Contact/5

    /// <summary>
    /// Deletes the specified contact
    /// </summary>
    /// <param name="id">Id of the contact</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        RestErrorResponse errorResponse;

        var contact = await _bll.Contacts.FirstOrDefaultAsync(id);

        if (contact == null)
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
                        $"Contact with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        Guid? associationId = null;
        if (contact.AssociationId != null)
        {
            associationId = contact.AssociationId.Value;
        }
        else if (contact.BuildingId != null)
        {
            var building = await _bll.Buildings.FirstOrDefaultAsync(contact.BuildingId.Value);
            associationId = building!.AssociationId;
        }

        if (associationId != null && !await _bll.Members.IsAdmin(personId, associationId.Value))
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
                        "User does not have permission to delete this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var person = (await _bll.Persons.FirstOrDefaultAsync(contact.PersonId!.Value))!;

        if (associationId == null && person.AppUserId != User.GetUserId())
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
                        "User does not have permission to delete this contact"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.Contacts.Remove(contact);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ContactExists(Guid id)
    {
        return await _bll.Contacts.ExistsAsync(id);
    }
}