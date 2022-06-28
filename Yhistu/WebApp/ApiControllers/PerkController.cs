#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PerkController : ControllerBase
{
    /*
    private readonly IAppBLL _bll;
    private readonly PerkMapper _perkMapper;

    public PerkController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _perkMapper = new PerkMapper(mapper);
    }

    // GET: api/Perk
    
    /// <summary>
    /// Returns all the perks
    /// </summary>
    /// <returns>List of perks</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Perk>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Perk>>> GetPerks()
    {
        return Ok((await _bll.Perks.GetAllAsync()).Select(x => _perkMapper.Map(x)).ToList());
    }


    // GET: api/Perk/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Perk>> GetPerk(Guid id)
    {
        var perk = await _bll.Perks.FindAsync(id);

        if (perk == null)
        {
            return NotFound();
        }

        return perk;
    }

    // PUT: api/Perk/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerk(Guid id, Perk perk)
    {
        if (id != perk.Id)
        {
            return BadRequest();
        }

        _bll.Entry(perk).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PerkExists(id))
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

    // POST: api/Perk
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Perk>> PostPerk(Perk perk)
    {
        _bll.Perks.Add(perk);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetPerk", new { id = perk.Id }, perk);
    }

    // DELETE: api/Perk/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerk(Guid id)
    {
        var perk = await _bll.Perks.FindAsync(id);
        if (perk == null)
        {
            return NotFound();
        }

        _bll.Perks.Remove(perk);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool PerkExists(Guid id)
    {
        return _bll.Perks.Any(e => e.Id == id);
    }
    */
}