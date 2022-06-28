#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class InvoiceController : ControllerBase
{
/*
    private readonly IAppBLL _bll;
    private readonly InvoiceMapper _invoiceMapper;

    public InvoiceController(IAppBLL bll, IMapper mapepr)
    {
        _bll = bll;
        _invoiceMapper = new InvoiceMapper(mapepr);
    }

    // GET: api/Invoice
    /// <summary>
    /// Returns all Invoices
    /// </summary>
    /// <returns>List of invoices</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Invoice>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Invoice>>> GetInvoices()
    {
        return Ok((await _bll.Invoices.GetAllAsync()).Select(x => _invoiceMapper.Map(x)).ToList());
    }

    // GET: api/Invoice/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        var invoice = await _bll.Invoices.FindAsync(id);

        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    // PUT: api/Invoice/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
    {
        if (id != invoice.Id)
        {
            return BadRequest();
        }

        _bll.Entry(invoice).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceExists(id))
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

    // POST: api/Invoice
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
    {
        _bll.Invoices.Add(invoice);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
    }

    // DELETE: api/Invoice/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        var invoice = await _bll.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        _bll.Invoices.Remove(invoice);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool InvoiceExists(Guid id)
    {
        return _bll.Invoices.Any(e => e.Id == id);
    }
    */
}