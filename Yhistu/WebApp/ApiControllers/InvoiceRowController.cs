#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InvoiceRowController : ControllerBase
{
/*
    private readonly IAppBLL _bll;
    private readonly InvoiceRowMapper _invoiceRowMapper;

    public InvoiceRowController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _invoiceRowMapper = new InvoiceRowMapper(mapper);
    }

    // GET: api/InvoiceRow
    /// <summary>
    /// Returns all the invoice rows
    /// </summary>
    /// <returns>List of invoice rows</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.InvoiceRow>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.InvoiceRow>>> GetInvoiceRows()
    {
        return Ok((await _bll.InvoiceRows.GetAllAsync()).Select(x => _invoiceRowMapper.Map(x)).ToList());
    }

    
    // GET: api/InvoiceRow/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceRow>> GetInvoiceRow(Guid id)
    {
        var invoiceRow = await _bll.InvoiceRows.FindAsync(id);

        if (invoiceRow == null)
        {
            return NotFound();
        }

        return invoiceRow;
    }

    // PUT: api/InvoiceRow/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoiceRow(Guid id, InvoiceRow invoiceRow)
    {
        if (id != invoiceRow.Id)
        {
            return BadRequest();
        }

        _bll.Entry(invoiceRow).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceRowExists(id))
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

    // POST: api/InvoiceRow
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<InvoiceRow>> PostInvoiceRow(InvoiceRow invoiceRow)
    {
        _bll.InvoiceRows.Add(invoiceRow);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetInvoiceRow", new { id = invoiceRow.Id }, invoiceRow);
    }

    // DELETE: api/InvoiceRow/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoiceRow(Guid id)
    {
        var invoiceRow = await _bll.InvoiceRows.FindAsync(id);
        if (invoiceRow == null)
        {
            return NotFound();
        }

        _bll.InvoiceRows.Remove(invoiceRow);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool InvoiceRowExists(Guid id)
    {
        return _bll.InvoiceRows.Any(e => e.Id == id);
    }
    */
}