#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InvoiceTypeController : ControllerBase
{
/*
    private readonly IAppBLL _bll;
    private readonly InvoiceTypeMapper _invoiceTypeMapper;

    public InvoiceTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _invoiceTypeMapper = new InvoiceTypeMapper(mapper);
    }

    // GET: api/InvoiceType
    
    /// <summary>
    /// Returns all invoice types
    /// </summary>
    /// <returns>List of invoice types</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.InvoiceType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.InvoiceType>>> GetInvoiceTypes()
    {
        return Ok((await _bll.InvoiceTypes.GetAllAsync()).Select(x => _invoiceTypeMapper.Map(x)).ToList());
    }

    
    // GET: api/InvoiceType/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceType>> GetInvoiceType(Guid id)
    {
        var invoiceType = await _bll.InvoiceTypes.FindAsync(id);

        if (invoiceType == null)
        {
            return NotFound();
        }

        return invoiceType;
    }

    // PUT: api/InvoiceType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoiceType(Guid id, InvoiceType invoiceType)
    {
        if (id != invoiceType.Id)
        {
            return BadRequest();
        }

        _bll.Entry(invoiceType).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceTypeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/InvoiceType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<InvoiceType>> PostInvoiceType(InvoiceType invoiceType)
    {
        _bll.InvoiceTypes.Add(invoiceType);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetInvoiceType", new { id = invoiceType.Id }, invoiceType);
    }

    // DELETE: api/InvoiceType/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoiceType(Guid id)
    {
        var invoiceType = await _bll.InvoiceTypes.FindAsync(id);
        if (invoiceType == null)
        {
            return NotFound();
        }

        _bll.InvoiceTypes.Remove(invoiceType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool InvoiceTypeExists(Guid id)
    {
        return _bll.InvoiceTypes.Any(e => e.Id == id);
    }
    */
}