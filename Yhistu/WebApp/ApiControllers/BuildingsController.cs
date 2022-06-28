#nullable disable
using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
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
public class BuildingsController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly BuildingMapper _buildingMapper;

    public BuildingsController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _buildingMapper = new BuildingMapper(mapper);
    }

    // GET: api/Buildings

    /// <summary>
    /// Returns all the buildings connected to the association provided
    /// </summary>
    /// <param name="associationId">The Id of the association</param>
    /// <returns>All the buildings of the association</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Building>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Building>>> GetBuildings(Guid? associationId)
    {
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
                        "Association Id not specified"
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
                Title = "Unauthorized",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Unauthorized"] = new List<string>
                    {
                        "User does not have permission to view this data"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        if (!await _bll.Members.IsMember(personId, associationId.Value))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "User is not a member of the association"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var buildings = (await _bll.Buildings.GetAllAsync(associationId.Value))!.Select(x => _buildingMapper.Map(x))
            .ToList();


        foreach (var each in buildings)
        {
            each.Address = (await _bll.Contacts.GetBuildingAddress(each.Id))?.Value;
        }

        return Ok(buildings);
    }

    // GET: api/Buildings/Details/5

    /// <summary>
    /// Gets the details of a particular building 
    /// </summary>
    /// <param name="id">Id of the requested building</param>
    /// <returns>The requested building object</returns>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Building), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<App.Public.DTO.v1.Building>> GetBuilding(Guid id)
    {
        var building = _buildingMapper.Map(await _bll.Buildings.FirstOrDefaultAsync(id));

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

        if (!await _bll.Members.IsMember(User.GetUserId(), building.AssociationId))
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
                        "User does not have permission to view this building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        building.Address = (await _bll.Contacts.GetBuildingAddress(building.Id))?.Value;

        return building;
    }


    // PUT: api/Buildings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified building
    /// </summary>
    /// <param name="id">Id of the building</param>
    /// <param name="building">Supply updated data for the building</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutBuilding(Guid id, App.Public.DTO.v1.Building building)
    {
        if (id != building.Id)
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

        if (!await _bll.Buildings.ExistsAsync(building.Id))
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
                        $"Building with the id {id} was not found"
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
                        "User does not have permission to update this building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        if (building.Address != null)
        {
            var addressDb = await _bll.Contacts.GetBuildingAddress(building.Id);

            if (addressDb != null)
            {
                addressDb.Value = building.Address;
                _bll.Contacts.Update(addressDb);
            }
            else
            {
                var address = new App.BLL.DTO.Contact
                {
                    ContactTypeId = await _bll.ContactTypes.GetAddressTypeId(),
                    BuildingId = building.Id,
                    Value = building.Address,
                };

                _bll.Contacts.Add(address);
            }

            await _bll.SaveChangesAsync();
        }

        var bllEntity = _buildingMapper.Map(building)!;
        _bll.Buildings.Update(bllEntity);
        await _bll.SaveChangesAsync();

        await _bll.Buildings.CalculateAreas(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Buildings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new building connected to the association provided
    /// </summary>
    /// <param name="building">Supply data for the building</param>
    /// <returns>Newly created building</returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Building), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<App.Public.DTO.v1.Building>> PostBuilding(App.Public.DTO.v1.Building building)
    {
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
                        "User does not have permission to add a new building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _buildingMapper.Map(building)!;
        _bll.Buildings.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Buildings.GetIdFromDb(bllEntity);

        await _bll.Buildings.CalculateAreas(bllEntity);
        await _bll.SaveChangesAsync();

        if (building.Address != null)
        {
            var address = new App.BLL.DTO.Contact
            {
                ContactTypeId = await _bll.ContactTypes.GetAddressTypeId(),
                BuildingId = bllEntity.Id,
                Value = building.Address,
            };

            _bll.Contacts.Add(address);
            await _bll.SaveChangesAsync();
        }

        return CreatedAtAction("GetBuilding", new {id = bllEntity.Id}, bllEntity);
    }


    // DELETE: api/Buildings/5

    /// <summary>
    /// Deletes the specified building and all the apartments, contacts,
    /// meters, and person-apartment connection associated with the building
    /// </summary>
    /// <param name="id">Id of the building</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBuilding(Guid id)
    {
        RestErrorResponse errorResponse;

        var buildingDb = await _bll.Buildings.FirstOrDefaultAsync(id);

        if (buildingDb == null)
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
                        $"Building with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, buildingDb.AssociationId))
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
                        "User does not have permission to delete this building"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var contacts = await _bll.Contacts.GetAllBuildingAsync(id);
        foreach (var each in contacts)
        {
            _bll.Contacts.Remove(each);
        }

        await _bll.SaveChangesAsync();

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

        var apartments = await _bll.Apartments.GetAllInBuildingAsync(id);

        foreach (var apartment in apartments)
        {
            var meters = await _bll.Meters.GetAllAsync(apartment.Id, "apartment");
            foreach (var each in meters)
            {
                foreach (var reading in await _bll.MeterReadings.GetAllAsync(each.Id))
                {
                    _bll.MeterReadings.Remove(reading);
                }

                _bll.Meters.Remove(each);
            }

            await _bll.SaveChangesAsync();

            var apartmentPersons = await _bll.ApartmentPersons.GetAllApartmentAsync(apartment.Id);
            foreach (var person in apartmentPersons)
            {
                _bll.ApartmentPersons.Remove(person);
            }

            await _bll.SaveChangesAsync();

            var apartmentDb = await _bll.Apartments.FirstOrDefaultAsync(id);

            _bll.Apartments.Remove(apartment);
            await _bll.SaveChangesAsync();
        }


        _bll.Buildings.Remove(buildingDb);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> BuildingExists(Guid id)
    {
        return await _bll.Buildings.ExistsAsync(id);
    }
}