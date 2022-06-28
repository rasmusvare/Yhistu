#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RelationshipTypeController : ControllerBase
{
    /*
    private readonly IAppBLL _bll;
    private readonly RelationShipTypeMapper _relationShipTypeMapper;

    public RelationshipTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _relationShipTypeMapper = new RelationShipTypeMapper(mapper);

    }

    // GET: api/RelationshipType
    
    /// <summary>
    /// Returns all relationship types
    /// </summary>
    /// <returns>List of relationship types</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.RelationshipType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.RelationshipType>>> GetRelationshipTypes()
    {
        return Ok((await _bll.RelationshipTypes.GetAllAsync()).Select(x => _relationShipTypeMapper.Map(x)).ToList());
    }

    
    // GET: api/RelationshipType/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RelationshipType>> GetRelationshipType(Guid id)
    {
        var relationshipType = await _bll.RelationshipTypes.FindAsync(id);

        if (relationshipType == null)
        {
            return NotFound();
        }

        return relationshipType;
    }

    // PUT: api/RelationshipType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRelationshipType(Guid id, RelationshipType relationshipType)
    {
        if (id != relationshipType.Id)
        {
            return BadRequest();
        }

        _bll.Entry(relationshipType).State = EntityState.Modified;

        try
        {
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RelationshipTypeExists(id))
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

    // POST: api/RelationshipType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<RelationshipType>> PostRelationshipType(RelationshipType relationshipType)
    {
        _bll.RelationshipTypes.Add(relationshipType);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetRelationshipType", new { id = relationshipType.Id }, relationshipType);
    }

    // DELETE: api/RelationshipType/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRelationshipType(Guid id)
    {
        var relationshipType = await _bll.RelationshipTypes.FindAsync(id);
        if (relationshipType == null)
        {
            return NotFound();
        }

        _bll.RelationshipTypes.Remove(relationshipType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private bool RelationshipTypeExists(Guid id)
    {
        return _bll.RelationshipTypes.Any(e => e.Id == id);
    }
    */
}