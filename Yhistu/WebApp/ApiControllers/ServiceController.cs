#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ServiceController : ControllerBase
{
    /*
  private readonly IAppBLL _bll;
  private readonly ServiceMapper _serviceMapper;

  public ServiceController(IAppBLL bll, IMapper mapper)
  {
      _bll = bll;
      _serviceMapper = new ServiceMapper(mapper);
  }

  // GET: api/Service
  
  /// <summary>
  /// Returns all the services
  /// </summary>
  /// <returns>List of services</returns>
  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Service>), StatusCodes.Status200OK)]
  [ProducesErrorResponseType(typeof(RestErrorResponse))]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Service>>> GetServices()
  {
      return (await _bll.Services.GetAllAsync()).Select(x=>_serviceMapper.Map(x)).ToList();
  }


  // GET: api/Service/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Service>> GetService(Guid id)
  {
      var service = await _bll.Services.FindAsync(id);

      if (service == null)
      {
          return NotFound();
      }

      return service;
  }

  // PUT: api/Service/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutService(Guid id, Service service)
  {
      if (id != service.Id)
      {
          return BadRequest();
      }

      _bll.Entry(service).State = EntityState.Modified;

      try
      {
          await _bll.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
          if (!ServiceExists(id))
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

  // POST: api/Service
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Service>> PostService(Service service)
  {
      _bll.Services.Add(service);
      await _bll.SaveChangesAsync();

      return CreatedAtAction("GetService", new { id = service.Id }, service);
  }

  // DELETE: api/Service/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteService(Guid id)
  {
      var service = await _bll.Services.FindAsync(id);
      if (service == null)
      {
          return NotFound();
      }

      _bll.Services.Remove(service);
      await _bll.SaveChangesAsync();

      return NoContent();
  }

  private bool ServiceExists(Guid id)
  {
      return _bll.Services.Any(e => e.Id == id);
  }
  */
}