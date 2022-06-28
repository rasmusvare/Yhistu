#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CalculationRulesController : ControllerBase
{
    /*
    private readonly IAppBLL _bll;
    private readonly CalculationRulesMapper _calculationRulesMapper;

    public CalculationRulesController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _calculationRulesMapper = new CalculationRulesMapper(mapper);
    }

    // GET: api/CalculationRules
    /// <summary>
    /// Returns all the calculation rules. NOT IMPLEMENTED
    /// </summary>
    /// <returns>List of calculation rules</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CalculationRules>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.CalculationRules>>> GetCalculationRules()
    {
        return Ok((await _bll.CalculationRules.GetAllAsync()).Select(x => _calculationRulesMapper.Map(x)));
    }

    // GET: api/CalculationRules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CalculationRules>> GetCalculationRules(Guid id)
    {
        var calculationRules = await _bll.CalculationRules.FindAsync(id);

        if (calculationRules == null)
        {
            return NotFound();
        }

        return calculationRules;
    }

    // PUT: api/CalculationRules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCalculationRules(Guid id, CalculationRules calculationRules)
    {
        if (id != calculationRules.Id)
        {
            return BadRequest();
        }

        _bll.Entry(calculationRules).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CalculationRulesExists(id))
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

    // POST: api/CalculationRules
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CalculationRules>> PostCalculationRules(CalculationRules calculationRules)
    {
        _bll.CalculationRules.Add(calculationRules);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetCalculationRules", new { id = calculationRules.Id }, calculationRules);
    }

    // DELETE: api/CalculationRules/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalculationRules(Guid id)
    {
        var calculationRules = await _bll.CalculationRules.FindAsync(id);
        if (calculationRules == null)
        {
            return NotFound();
        }

        _bll.CalculationRules.Remove(calculationRules);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool CalculationRulesExists(Guid id)
    {
        return _bll.CalculationRules.Any(e => e.Id == id);
    }
    */
}