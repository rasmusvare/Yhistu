#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ServiceProviderController : ControllerBase
{
    /*
   private readonly IAppBLL _bll;
   private readonly ServiceProviderMapper _serviceProviderMapper;

   public ServiceProviderController(IAppBLL bll, IMapper mapper)
   {
       _bll = bll;
       _serviceProviderMapper = new ServiceProviderMapper(mapper);
   }

   // GET: api/ServiceProvider
   
   /// <summary>
   /// Returns all the service providers
   /// </summary>
   /// <returns>List of service providers</returns>
   [HttpGet]
   [Produces("application/json")]
   [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.ServiceProvider>), StatusCodes.Status200OK)]
   [ProducesErrorResponseType(typeof(RestErrorResponse))]
   [ProducesResponseType(StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.ServiceProvider>>> GetServiceProviders()
   {
       return Ok((await _bll.ServiceProviders.GetAllAsync()).Select(x => _serviceProviderMapper.Map(x)).ToList());
   }

  
   // GET: api/ServiceProvider/5
   [HttpGet("{id}")]
   public async Task<ActionResult<App.Public.DTO.v1.ServiceProvider>> GetServiceProvider(Guid id)
   {
       var serviceProvider = await _bll.ServiceProviders.FindAsync(id);

       if (serviceProvider == null)
       {
           return NotFound();
       }

       return serviceProvider;
   }

   // PUT: api/ServiceProvider/5
   // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   [HttpPut("{id}")]
   public async Task<IActionResult> PutServiceProvider(Guid id, App.Public.DTO.v1.ServiceProvider serviceProvider)
   {
       if (id != serviceProvider.Id)
       {
           return BadRequest();
       }

       _bll.Entry(serviceProvider).State = EntityState.Modified;

       try
       {
           await _bll.SaveChangesAsync();
       }
       catch (DbUpdateConcurrencyException)
       {
           if (!ServiceProviderExists(id))
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

   // POST: api/ServiceProvider
   // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
   [HttpPost]
   public async Task<ActionResult<App.Public.DTO.v1.ServiceProvider>> PostServiceProvider(App.Public.DTO.v1.ServiceProvider serviceProvider)
   {
       _bll.ServiceProviders.Add(serviceProvider);
       await _bll.SaveChangesAsync();

       return CreatedAtAction("GetServiceProvider", new { id = serviceProvider.Id }, serviceProvider);
   }

   // DELETE: api/ServiceProvider/5
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteServiceProvider(Guid id)
   {
       var serviceProvider = await _bll.ServiceProviders.FindAsync(id);
       if (serviceProvider == null)
       {
           return NotFound();
       }

       _bll.ServiceProviders.Remove(serviceProvider);
       await _bll.SaveChangesAsync();

       return NoContent();
   }

   private bool ServiceProviderExists(Guid id)
   {
       return _bll.ServiceProviders.Any(e => e.Id == id);
   }
   */
}