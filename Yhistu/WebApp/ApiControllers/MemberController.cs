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
public class MemberController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly MemberMapper _memberMapper;

    public MemberController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _memberMapper = new MemberMapper(mapper);
    }

    // GET: api/Member

    /// <summary>
    /// Returns all members of the association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of members</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Member>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Member>>> GetMembers(Guid associationId)
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
                        "User does not have permission to view the members of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        IEnumerable<Member> members;
        if (await _bll.Members.IsAdmin(personId, associationId))
        {
            members = (await _bll.Members.GetAllAsync(associationId)).Select(x => _memberMapper.Map(x)).ToList();
            return Ok(members);
        }

        members = (await _bll.Members.GetBoardMembersAsync(associationId)).Select(x => _memberMapper.Map(x)).ToList();
        return Ok(members);
    }

    // GET: api/Member/details/5

    /// <summary>
    /// Gets the details of a particular member
    /// </summary>
    /// <param name="id">Id of the member</param>
    /// <returns>Member object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Member), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Member>> GetMember(Guid id)
    {
        var member = await _bll.Members.FirstOrDefaultAsync(id);

        if (member == null)
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
                        "Member not not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, member.AssociationId) || member.PersonId == personId)
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
                        "User does not have permission to view this member"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_memberMapper.Map(member));
    }

    // GET: api/Member/details/5

    /// <summary>
    /// If user is an administrator of the association,
    /// switches roles between regular user and admin user.
    /// </summary>
    /// <param name="associationId">Id of the member</param>
    /// <returns></returns>
    [HttpGet("role/{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Member), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SwitchRole(Guid associationId)
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
                        $"Association with id {associationId} not not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var member = await _bll.Members.Find(personId, associationId);

        if (member == null)
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
                        "User is not a member of the association"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (!member.MemberType!.IsAdministrator && !member.MemberType!.IsMemberOfBoard)
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
                        "User is not an administrator and thus cannot switch roles"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        member.ViewAsRegularUser = !member.ViewAsRegularUser;
        _bll.Members.Update(member);

        return Ok();
    }


    // PUT: api/Member/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified member
    /// </summary>
    /// <param name="id">Id of the member</param>
    /// <param name="member">Supply updated data for the member</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMember(Guid id, Member member)
    {
        if (id != member.Id)
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

        var memberDb = await _bll.Members.FirstOrDefaultAsync(id);

        if (memberDb == null)
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
                        $"Member with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, memberDb.AssociationId))
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

        _bll.Members.Update(_memberMapper.Map(member)!);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Member
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new member  
    /// </summary>
    /// <param name="member">Supply data for the new member</param>
    /// <returns>The newly created member object</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Member), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Member>> PostMember(Member member)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, member.AssociationId))
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
                        "User does not have permission to add a new member"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _memberMapper.Map(member)!;
        _bll.Members.Add(bllEntity);
        await _bll.SaveChangesAsync();
        member.Id = _bll.Members.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetMember", new {id = member.Id}, member);
    }


    // DELETE: api/Member/5

    /// <summary>
    /// Deletes the specified member
    /// </summary>
    /// <param name="id">Id of the member</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMember(Guid id)
    {
        RestErrorResponse errorResponse;

        var member = await _bll.Members.FirstOrDefaultAsync(id);

        if (member == null)
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
                        $"Member with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        if (await _bll.Members.IsAdmin(member.PersonId, member.AssociationId))
        {
            _bll.Members.Remove(member);
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
                    "User does not have permission to delete this member"
                }
            }
        };
        return Unauthorized(errorResponse);
    }

    private async Task<bool> MemberExists(Guid id)
    {
        return await _bll.Members.ExistsAsync(id);
    }
}