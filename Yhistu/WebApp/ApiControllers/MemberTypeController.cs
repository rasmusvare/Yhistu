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
public class MemberTypeController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MemberTypeMapper _memberTypeMapper;

    public MemberTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _memberTypeMapper = new MemberTypeMapper(mapper);
    }

    // GET: api/MemberType

    /// <summary>
    /// Returns all the member types of the association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of member types</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.MemberType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.MemberType>>> GetMemberTypes(Guid associationId)
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
                        "User does not have permission to view the member types of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok((await _bll.MemberTypes.GetAllAsync(associationId)).Select(x => _memberTypeMapper.Map(x)).ToList());
    }

    // GET: api/MemberType/5
    /// <summary>
    /// Get the details of a particular member type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MemberType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberType>> GetMemberType(Guid id)
    {
        var memberType = await _bll.MemberTypes.FirstOrDefaultAsync(id);

        if (memberType == null)
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
                        "Meter type not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, memberType.AssociationId))
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

        return Ok(_memberTypeMapper.Map(memberType));
    }

    // PUT: api/MemberType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified member type
    /// </summary>
    /// <param name="id"></param>
    /// <param name="memberType"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMemberType(Guid id, MemberType memberType)
    {
        if (id != memberType.Id)
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

        var memberTypeDb = await _bll.MemberTypes.FirstOrDefaultAsync(id);

        if (memberTypeDb?.AssociationId == null)
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
                        $"Member type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, memberTypeDb.AssociationId))
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
                        "User does not have permission to update this member type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.MemberTypes.Update(_memberTypeMapper.Map(memberType)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/MemberType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new member type
    /// </summary>
    /// <param name="memberType">Supply data for the new member type</param>
    /// <returns>Newly created member type</returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.MemberType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MemberType>> PostMemberType(MemberType memberType)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, memberType.AssociationId))
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
                        "User does not have permission to add a new member type"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _memberTypeMapper.Map(memberType)!;
        _bll.MemberTypes.Add(bllEntity);
        await _bll.SaveChangesAsync();
        memberType.Id = _bll.MemberTypes.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMemberType", new {id = memberType.Id}, memberType);
    }

    // DELETE: api/MemberType/5

    /// <summary>
    /// Deletes the specified member type
    /// </summary>
    /// <param name="id">Id of the member type</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMemberType(Guid id)
    {
        var memberType = await _bll.MemberTypes.FirstOrDefaultAsync(id);

        if (memberType == null)
        {
            return NotFound();
        }
        
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());
        
        RestErrorResponse errorResponse;
        if (await _bll.Members.IsAdmin(personId, memberType.AssociationId))
        {
            if (await _bll.MemberTypes.IsUsed(id))
            {
                _bll.MemberTypes.Remove(memberType);
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
                        "Please remove this member type from all members before deleting"
                    }
                }
            };
            return BadRequest(errorResponse);
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
                    "User does not have permission to delete this member type"
                }
            }
        };

        return Unauthorized(errorResponse);
    }

    private async Task<bool> MemberTypeExists(Guid id)
    {
        return await _bll.MemberTypes.ExistsAsync(id);
    }
}