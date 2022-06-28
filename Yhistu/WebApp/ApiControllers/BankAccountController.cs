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

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BankAccountController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly BankAccountMapper _bankAccountMapper;

    public BankAccountController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _bankAccountMapper = new BankAccountMapper(mapper);
    }

    // GET: api/BankAccount

    /// <summary>
    /// Returns all the bank accounts of the association
    /// </summary>
    /// <param name="associationId">Id of the association</param>
    /// <returns>List of bank accounts</returns>
    [HttpGet("{associationId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.BankAccount>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.BankAccount>>> GetBankAccounts(Guid associationId)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, associationId))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Title = "Bad request",
                Status = HttpStatusCode.Unauthorized,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        "User does not have permission to view the bank accounts of this association"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var res = (await _bll.BankAccounts.GetAllAsync(associationId)).Select(x => _bankAccountMapper.Map(x)).ToList();
        return Ok(res);
    }

    // GET: api/BankAccount/Details/5

    /// <summary>
    /// Gets the details of a particular bank account
    /// </summary>
    /// <param name="id">Id of the bank account</param>
    /// <returns>Bank account object</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.BankAccount), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BankAccount>> GetBankAccount(Guid id)
    {
        var bankAccount = await _bll.BankAccounts.FirstOrDefaultAsync(id);

        if (bankAccount == null)
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
                        "Bank account not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsMember(personId, bankAccount.AssociationId))
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
                        "User does not have permission to view this bank account"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        return Ok(_bankAccountMapper.Map(bankAccount));
    }

    // PUT: api/BankAccount/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the data of the specified bank account
    /// </summary>
    /// <param name="id">Id of the bank account</param>
    /// <param name="bankAccount">Supply updted data for the bank account</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutBankAccount(Guid id, BankAccount bankAccount)
    {
        if (id != bankAccount.Id)
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

        var bankAccountDb = await _bll.BankAccounts.FirstOrDefaultAsync(id);

        if (bankAccountDb?.AssociationId == null)
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
                        $"Bank account with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, bankAccountDb.AssociationId))
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
                        "User does not have permission to update this bank account"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        _bll.BankAccounts.Update(_bankAccountMapper.Map(bankAccount)!);
        await _bll.SaveChangesAsync();

        return Ok();
    }

    // POST: api/BankAccount
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new bank account connected with the association
    /// </summary>
    /// <param name="bankAccount">Supply data for the new bank account</param>
    /// <returns>The newly created bank account</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Building), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
    {
        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (!await _bll.Members.IsAdmin(personId, bankAccount.AssociationId))
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
                        "User does not have permission to add a new bank account"
                    }
                }
            };
            return Unauthorized(errorResponse);
        }

        var bllEntity = _bankAccountMapper.Map(bankAccount)!;
        _bll.BankAccounts.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bankAccount.Id = _bll.BankAccounts.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetBankAccount", new {id = bankAccount.Id}, bankAccount);
    }

    // DELETE: api/BankAccount/5

    /// <summary>
    /// Deletes the specified bank account
    /// </summary>
    /// <param name="id">Id f the bank account</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBankAccount(Guid id)
    {
        var bankAccount = await _bll.BankAccounts.FirstOrDefaultAsync(id);

        RestErrorResponse errorResponse;
        if (bankAccount == null)
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
                        $"Bank account with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }


        var personId = await _bll.Persons.GetMainPersonId(User.GetUserId());

        if (await _bll.Members.IsAdmin(personId, bankAccount.AssociationId))
        {
            _bll.BankAccounts.Remove(bankAccount);
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
                    "User does not have permission to delete this bank account"
                }
            }
        };

        return Unauthorized(errorResponse);
    }

    private async Task<bool> BankAccountExists(Guid id)
    {
        return await _bll.BankAccounts.ExistsAsync(id);
    }
}