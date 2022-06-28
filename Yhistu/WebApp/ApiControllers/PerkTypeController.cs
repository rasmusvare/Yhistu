#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PerkTypeController : ControllerBase
{
    /*
    private readonly IAppBLL _bll;
    private readonly PerkTypeMapper _perkTypeMapper;

    public PerkTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _perkTypeMapper = new PerkTypeMapper(mapper);

    }

    // GET: api/PerkType
    
    /// <summary>
    /// Returns all the perk types
    /// </summary>
    /// <returns>List of perk types</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.PerkType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.PerkType>>> GetPerkTypes()
    {
        return Ok((await _bll.PerkTypes.GetAllAsync()).Select(x => _perkTypeMapper.Map(x)).ToList());
    }

    
    // GET: api/PerkType/5
    [HttpGet("details/{id}")]
    public async Task<ActionResult<PerkType>> GetPerkType(Guid id)
    {
        var perkType = await _bll.PerkTypes.FindAsync(id);

        if (perkType == null)
        {
            return NotFound();
        }

        return perkType;
    }

    // PUT: api/PerkType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerkType(Guid id, PerkType perkType)
    {
        if (id != perkType.Id)
        {
            return BadRequest();
        }

        _bll.Entry(perkType).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PerkTypeExists(id))
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

    // POST: api/PerkType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<PerkType>> PostPerkType(PerkType perkType)
    {
        _bll.PerkTypes.Add(perkType);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetPerkType", new { id = perkType.Id }, perkType);
    }

    // DELETE: api/PerkType/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerkType(Guid id)
    {
        var perkType = await _bll.PerkTypes.FindAsync(id);
        if (perkType == null)
        {
            return NotFound();
        }

        _bll.PerkTypes.Remove(perkType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool PerkTypeExists(Guid id)
    {
        return _bll.PerkTypes.Any(e => e.Id == id);
    }
    */
}