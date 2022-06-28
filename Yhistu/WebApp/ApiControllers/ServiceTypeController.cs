#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ServiceTypeController : ControllerBase
{
    /*
  private readonly IAppBLL _bll;
  private readonly ServiceTypeMapper _serviceTypeMapper;

  public ServiceTypeController(IAppBLL bll, IMapper mapper)
  {
      _bll = bll;
      _serviceTypeMapper = new ServiceTypeMapper(mapper);
  }

  // GET: api/ServiceType
  
  /// <summary>
  /// Returns all service types
  /// </summary>
  /// <returns>List of service types</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ServiceType>), StatusCodes.Status200OK)]
  [ProducesErrorResponseType(typeof(RestErrorResponse))]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ServiceType>>> GetServiceTypes()
  {
      return Ok((await _bll.ServiceTypes.GetAllAsync()).Select(x => _serviceTypeMapper.Map(x)).ToList());
  }


  // GET: api/ServiceType/5
  [HttpGet("{id}")]
  public async Task<ActionResult<ServiceType>> GetServiceType(Guid id)
  {
      var serviceType = await _bll.ServiceTypes.FindAsync(id);

      if (serviceType == null)
      {
          return NotFound();
      }

      return serviceType;
  }

  // PUT: api/ServiceType/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutServiceType(Guid id, ServiceType serviceType)
  {
      if (id != serviceType.Id)
      {
          return BadRequest();
      }

      _bll.Entry(serviceType).State = EntityState.Modified;

      try
      {
          await _bll.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
          if (!ServiceTypeExists(id))
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

  // POST: api/ServiceType
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<ServiceType>> PostServiceType(ServiceType serviceType)
  {
      _bll.ServiceTypes.Add(serviceType);
      await _bll.SaveChangesAsync();

      return CreatedAtAction("GetServiceType", new { id = serviceType.Id }, serviceType);
  }

  // DELETE: api/ServiceType/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteServiceType(Guid id)
  {
      var serviceType = await _bll.ServiceTypes.FindAsync(id);
      if (serviceType == null)
      {
          return NotFound();
      }

      _bll.ServiceTypes.Remove(serviceType);
      await _bll.SaveChangesAsync();

      return NoContent();
  }

  private bool ServiceTypeExists(Guid id)
  {
      return _bll.ServiceTypes.Any(e => e.Id == id);
  }
  */
}