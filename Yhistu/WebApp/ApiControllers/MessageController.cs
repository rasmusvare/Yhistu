#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MessageController : ControllerBase
{
    /*
 private readonly IAppBLL _bll;
 private readonly MessageMapper _messageMapper;

 public MessageController(IAppBLL bll, IMapper mapper)
 {
     _bll = bll;
     _messageMapper = new MessageMapper(mapper);
 }

 // GET: api/Message
 
 /// <summary>
 /// Returns all the messages
 /// </summary>
 /// <returns>List of messages</returns>
 [HttpGet, Produces("application/json"),
  ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Message>), StatusCodes.Status200OK),
  ProducesErrorResponseType(typeof(RestErrorResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized),
  ProducesResponseType(StatusCodes.Status404NotFound)]
 public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Message>>> GetMessages()
 {
     return Ok((await _bll.Messages.GetAllAsync()).Select(x=>_messageMapper.Map(x)).ToList());
 }


 // GET: api/Message/5
 [HttpGet("{id}")]
 public async Task<ActionResult<Message>> GetMessage(Guid id)
 {
     var message = await _bll.Messages.FindAsync(id);

     if (message == null)
     {
         return NotFound();
     }

     return message;
 }

 // PUT: api/Message/5
 // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
 [HttpPut("{id}")]
 public async Task<IActionResult> PutMessage(Guid id, Message message)
 {
     if (id != message.Id)
     {
         return BadRequest();
     }

     _bll.Entry(message).State = EntityState.Modified;

     try
     {
         await _bll.SaveChangesAsync();
     }
     catch (DbUpdateConcurrencyException)
     {
         if (!MessageExists(id))
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

 // POST: api/Message
 // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
 [HttpPost]
 public async Task<ActionResult<Message>> PostMessage(Message message)
 {
     _bll.Messages.Add(message);
     await _bll.SaveChangesAsync();

     return CreatedAtAction("GetMessage", new {id = message.Id}, message);
 }

 // DELETE: api/Message/5
 [HttpDelete("{id}")]
 public async Task<IActionResult> DeleteMessage(Guid id)
 {
     var message = await _bll.Messages.FindAsync(id);
     if (message == null)
     {
         return NotFound();
     }

     _bll.Messages.Remove(message);
     await _bll.SaveChangesAsync();

     return NoContent();
 }

 private bool MessageExists(Guid id)
 {
     return _bll.Messages.Any(e => e.Id == id);
 }
 */
}