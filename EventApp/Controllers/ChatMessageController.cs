using EventApp.Services.ChatMessage;
using EventApp.Shared.DTOs.Message;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatMessageController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Get or create a conversation between admin and organizer
        [HttpPost("conversation")]
        public async Task<ActionResult<ConversationDto>> GetOrCreateConversation([FromBody] CreateConversationDto dto)
        {
            var conversation = await _chatService.GetConversationAsync(dto.AdminId, dto.OrganizerId);
            if (conversation == null)
            {
                conversation = await _chatService.CreateConversationAsync(dto);
            }
            return Ok(conversation);
        }


        [HttpGet("my-conversations/{userId}")]
        public async Task<ActionResult<List<ConversationDto>>> GetUserConversations(Guid userId)
        {
            var conversations = await _chatService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }
        // Send a new message
        [HttpPost("send")]
        public async Task<ActionResult<MessageDto>> SendMessage([FromBody] SendMessageDto dto)
        {
            var sentMessage = await _chatService.SendMessageAsync(dto);
            return Ok(sentMessage);
        }

        // Mark a message as read
        [HttpPost("read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            await _chatService.MarkMessageAsReadAsync(messageId);
            return NoContent();
        }

        [HttpGet("conversation/{conversationId}/{currentUserid}")]
        public async Task<ActionResult<ConversationDto>> GetConversationById(int conversationId, Guid currentUserid)
        {
            var conversation = await _chatService.GetConversationByIdAsync(conversationId, currentUserid);
            if (conversation == null)
                return NotFound();

            return Ok(conversation);
        }
    }
}
