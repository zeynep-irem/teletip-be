using Microsoft.AspNetCore.Mvc;
using Teletipbe.Models;
using Teletipbe.Services;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetMessages(string sessionId)
        {
            var messages = await _messageService.GetMessagesAsync(sessionId);
            return Ok(messages);
        }

        [HttpPost("{sessionId}/send")]
        public async Task<IActionResult> SendMessage(string sessionId, [FromBody] MessageModel model)
        {
            await _messageService.SendMessageAsync(sessionId, model);
            return Ok(new { message = "Message sent successfully" });
        }
    }
}
