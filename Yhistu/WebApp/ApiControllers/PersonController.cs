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
public class PersonController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly IMapper _mapper;
    private readonly PersonMapper _personMapper;

    public PersonController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _mapper = mapper;
        _personMapper = new PersonMapper(mapper);
    }


    // GET: api/Person

    /// <summary>
    /// Returns all the persons connected with the user
    /// </summary>
    /// <returns>List of persons</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Person), 200)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Person>>> GetPersons()
    {
        return (await _bll.Persons.GetAllAsync(User.GetUserId())).Select(x => _personMapper.Map(x)).ToList();
    }


    // GET: api/Person

    /// <summary>
    /// Returns all the persons connected with the association
    /// </summary>
    /// <param name="associationId">Supply the Id of the association</param>
    /// <returns>List of persons</returns>
    [HttpGet("association/{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Person), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Person>>> GetPersons(Guid associationId)
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
                        "User does not have permission to view this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

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

        IEnumerable<App.Public.DTO.v1.Person> persons;

        if (await _bll.Members.IsAdmin(personId, associationId))
        {
            persons = (await _bll.Persons.GetAllAssociationAsync(associationId))
                .Select(x => _personMapper.Map(x))
                .ToList();

            return Ok(persons);
        }

        persons = (await _bll.Persons.GetBoardMembersAsync(associationId))
            .Select(x => _personMapper.Map(x))
            .ToList();
        return Ok(persons);
    }


    // GET: api/Person/5
    /// <summary>
    /// Gets the details of a particular person
    /// </summary>
    /// <param name="id">Id of the person</param>
    /// <returns>Person object</returns>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Person), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> GetPerson(Guid id)
    {
        var person = await _bll.Persons.FirstOrDefaultAsync(id);

        if (person == null)
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
                        "Person not not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (person.AppUserId == User.GetUserId())
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
                        "User does not have permission to view this person"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_personMapper.Map(person));
    }


    // PUT: api/Person/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified person
    /// </summary>
    /// <param name="id">Id of the person</param>
    /// <param name="person">Supply updated data for the person</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Person), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutPerson(Guid id, Person person)
    {
        if (id != person.Id)
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

        var personDb = await _bll.Persons.FirstOrDefaultAsync(id);

        if (personDb == null)
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
                        $"Person with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (personDb.AppUserId != User.GetUserId())
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
                        "User does not have permission to update this person"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.Persons.Update(_personMapper.Map(person)!);
        await _bll.SaveChangesAsync();

        return Ok();
    }


    // POST: api/Person
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new person
    /// </summary>
    /// <param name="person">Supply data for the new person</param>
    /// <returns>The newly created person object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Person), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<Person>> PostPerson(Person person)
    {
        var bllEntity = _personMapper.Map(person)!;

        var personDb = await _bll.Persons.FirstOrDefaultAsync(bllEntity);

        if (personDb == null)
        {
            bllEntity.AppUserId = User.GetUserId();
            _bll.Persons.Add(bllEntity);
            await _bll.SaveChangesAsync();
            bllEntity.Id = _bll.Persons.GetIdFromDb(bllEntity);

            var contact = new App.BLL.DTO.Contact
            {
                PersonId = bllEntity.Id,
                ContactTypeId = await _bll.ContactTypes.GetEmailTypeId(),
                Value = bllEntity.Email
            };

            _bll.Contacts.Add(contact);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new {id = bllEntity.Id}, bllEntity);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("PERSON EXISTS");

        // await _bll.SaveChangesAsync();

        return CreatedAtAction("GetPerson", new {id = personDb.Id}, personDb);
    }


    // DELETE: api/Person/5

    /// <summary>
    /// Deletes the specified person
    /// </summary>
    /// <param name="id">Id of the person</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var person = await _bll.Persons.FirstOrDefaultAsync(id);

        RestErrorResponse errorResponse;
        if (person == null)
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

        if (person.AppUserId == User.GetUserId() && !person.IsMain)
        {
            _bll.Persons.Remove(person);
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
                    "Cannot delete the main person connected with the user"
                }
            }
        };
        return BadRequest(errorResponse);
    }

    private Task<bool> PersonExists(Guid id)
    {
        return _bll.Persons.ExistsAsync(id);
    }
}