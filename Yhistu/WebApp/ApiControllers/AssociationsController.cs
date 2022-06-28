#nullable disable
using System.Diagnostics;
using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AssociationsController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly BankAccountMapper _bankAccountMapper;
    private readonly AssociationMapper _associationMapper;
    private readonly UserManager<App.Domain.Identity.AppUser> _userManager;
    private readonly SignInManager<App.Domain.Identity.AppUser> _signInManager;


    public AssociationsController(IAppBLL bll, IMapper mapper, UserManager<App.Domain.Identity.AppUser> userManager,
        SignInManager<App.Domain.Identity.AppUser> signInManager)
    {
        _bll = bll;
        _bankAccountMapper = new BankAccountMapper(mapper);
        _associationMapper = new AssociationMapper(mapper);
        _userManager = userManager;
        _signInManager = signInManager;
    }


    // GET: api/Associations

    /// <summary>
    /// Returns all the associations where the user is a member of.
    /// </summary>
    /// <returns>List of associations</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Association>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Association>>> GetAssociations()
    {
        Console.WriteLine(User.GetUserId());
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        var res = (await _bll.Associations.GetAllAsync(personId)).Select(a => _associationMapper.Map(a)).ToList();

        foreach (var association in res)
        {
            association.BankAccounts = (await _bll.BankAccounts.GetAllAsync(association.Id))
                .Select(p => _bankAccountMapper.Map(p)).ToList();
        }

        return Ok(res);
    }


    // GET: api/Associations/details/5

    /// <summary>
    /// Gets the details of a particular association
    /// </summary>
    /// <param name="id">Supply the Id of the association</param>
    /// <returns>Association object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Association>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<App.Public.DTO.v1.Association>> GetAssociation(Guid id)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, id))
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

        var association = await _bll.Associations.FirstOrDefaultAsync(id);


        if (association == null)
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
                        $"The association with id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        association.Address = await _bll.Contacts.GetAssociationAddress(id);

        return _associationMapper.Map(association);
    }


    // PUT: api/Associations/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified association
    /// </summary>
    /// <param name="id">Id of the association</param>
    /// <param name="association">Supply updated data for the association</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAssociation(Guid id, App.Public.DTO.v1.Association association)
    {
        if (id != association.Id)
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

        var associationDb = await _bll.Associations.FirstOrDefaultAsync(id);

        if (associationDb == null)
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
                        $"Association with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (!await _bll.Members.IsAdmin(User.GetUserId(), id))
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

        _bll.Associations.Update(_associationMapper.Map(association)!);

        await _bll.SaveChangesAsync();

        return Ok();
    }


    // POST: api/Associations
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new association
    /// </summary>
    /// <param name="association">Supply data for the new association</param>
    /// <returns>The newly created association</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Association), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<Association>> PostAssociation(App.Public.DTO.v1.Association association)
    {
        var bllEntity = _associationMapper.Map(association)!;
        _bll.Associations.Add(bllEntity);

        await _bll.SaveChangesAsync();

        association.Id = _bll.Associations.GetIdFromDb(bllEntity);
        var adminMemberType = _bll.MemberTypes.CreateDefaultAdminMemberType(association.Id);
        _bll.MemberTypes.Add(adminMemberType);

        await _bll.SaveChangesAsync();

        var adminMemberTypeId = _bll.MemberTypes.GetIdFromDb(adminMemberType);

        var member = new Member
        {
            AssociationId = association.Id,
            PersonId = await _bll.Persons.GetMainPersonId(User.GetUserId()),
            MemberTypeId = adminMemberTypeId,
            From = DateOnly.FromDateTime(DateTime.UtcNow),
        };

        _bll.Members.Add(member);
        await _bll.SaveChangesAsync();

        if (association.Address != null)
        {
            var address = new Contact
            {
                AssociationId = association.Id,
                ContactTypeId = await _bll.ContactTypes.GetAddressTypeId(),
                Value = association.Address
            };
            _bll.Contacts.Add(address);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetAssociation", new {id = association.Id}, association);
    }


    // DELETE: api/Associations/5

    /// <summary>
    /// Deletes the specified association and ALL related data
    /// </summary>
    /// <param name="id">Id of the association</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssociation(Guid id)
    {
        var association = await _bll.Associations.FirstOrDefaultAsync(id);

        if (association == null)
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
                        $"Association with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, id))
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
                        "User does not have permission to delete this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var contacts = await _bll.Contacts.GetAllAssociationAsync(id);
        foreach (var each in contacts)
        {
            _bll.Contacts.Remove(each);
            await _bll.SaveChangesAsync();
        }


        var bankAccounts = await _bll.BankAccounts.GetAllAsync(id);
        foreach (var each in bankAccounts)
        {
            _bll.BankAccounts.Remove(each);
            _bll.SaveChanges();
        }


        var members = (await _bll.Members.GetAllForDeleteAsync(id)).ToList();
        foreach (var member in members)
        {
            _bll.Members.Remove(member);
            await _bll.SaveChangesAsync();
        }

        var buildings = await _bll.Buildings.GetAllAsync(id);
        foreach (var building in buildings)
        {
            var buildingContacts = await _bll.Contacts.GetAllBuildingAsync(building.Id);
            foreach (var each in buildingContacts)
            {
                _bll.Contacts.Remove(each);
                await _bll.SaveChangesAsync();
            }

            var buildingMeters = await _bll.Meters.GetAllAsync(id, "building");
            if (buildingMeters != null)
            {
                foreach (var meter in buildingMeters)
                {
                    foreach (var reading in await _bll.MeterReadings.GetAllAsync(meter.Id))
                    {
                        _bll.MeterReadings.Remove(reading);
                    }

                    await _bll.SaveChangesAsync();
                }
            }

            var apartments = await _bll.Apartments.GetAllInBuildingAsync(building.Id);

            foreach (var apartment in apartments)
            {
                var apartmentMeters = await _bll.Meters.GetAllAsync(apartment.Id, "apartment");
                foreach (var each in apartmentMeters)
                {
                    foreach (var reading in await _bll.MeterReadings.GetAllAsync(each.Id))
                    {
                        _bll.MeterReadings.Remove(reading);
                        await _bll.SaveChangesAsync();
                    }

                    _bll.Meters.Remove(each);
                    await _bll.SaveChangesAsync();
                }


                var apartmentPersons = await _bll.ApartmentPersons.GetAllApartmentAsync(apartment.Id);
                foreach (var person in apartmentPersons)
                {
                    _bll.ApartmentPersons.Remove(person);
                    await _bll.SaveChangesAsync();
                }

                _bll.Apartments.Remove(apartment);
                await _bll.SaveChangesAsync();
            }

            _bll.Buildings.Remove(building);
            await _bll.SaveChangesAsync();
        }


        var contactTypes = await _bll.ContactTypes.GetAllAsync(id);
        foreach (var type in contactTypes)
        {
            if (type.AssociationId != null)
            {
                _bll.ContactTypes.Remove(type);
                await _bll.SaveChangesAsync();
            }
        }


        var meterTypes = await _bll.MeterTypes.GetAllAsync(id);
        foreach (var type in meterTypes)
        {
            if (type.AssociationId != null)
            {
                _bll.MeterTypes.Remove(type);
                await _bll.SaveChangesAsync();
            }
        }

        // var relationshipTypes = await _bll.RelationshipTypes.GetAllAsync(id);
        // foreach (var type in relationshipTypes)
        // {
        //     _bll.RelationshipTypes.Remove(type);
        // }

        // await _bll.SaveChangesAsync();

        var measuringUnits = await _bll.MeasuringUnits.GetAllAsync(id);
        foreach (var unit in measuringUnits)
        {
            if (unit.AssociationId != null)
            {
                _bll.MeasuringUnits.Remove(unit);
                await _bll.SaveChangesAsync();
            }
        }

        var memberTypes = await _bll.MemberTypes.GetAllAsync(id);
        foreach (var type in memberTypes)
        {
            _bll.MemberTypes.Remove(type);
            await _bll.SaveChangesAsync();
        }

        await _bll.Associations.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AssociationExists(Guid id)
    {
        return await _bll.Associations.ExistsAsync(id);
    }
}