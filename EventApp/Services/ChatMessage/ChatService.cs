using EventApp.Data;
using EventApp.Shared.DTOs.Message;
using EventApp.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Services.ChatMessage
{
    public class ChatService : IChatService
    {
        private readonly DataContext _ctx;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(DataContext ctx, IHubContext<ChatHub> hubcontext)
        {
            _ctx = ctx;
            _hubContext = hubcontext;
        }

        public async Task<ConversationDto> CreateConversationAsync(CreateConversationDto dto)
        {
            var conversation = new Conversation
            {
                AdminId = dto.AdminId,
                OrganizerId = dto.OrganizerId,
                Created = DateTime.UtcNow
            };

            _ctx.Conversations.Add(conversation);
            await _ctx.SaveChangesAsync();

            return new ConversationDto
            {
                Id = conversation.Id,
                AdminId = conversation.AdminId,
                AdminUsername = (await _ctx.Users.FindAsync(conversation.AdminId))?.Username,
                OrganizerId = conversation.OrganizerId,
                OrganizerUsername = (await _ctx.Users.FindAsync(conversation.OrganizerId))?.Username,
                Created = conversation.Created
            };
        }

        public async Task<ConversationDto?> GetConversationAsync(Guid adminId, Guid organizerId)
        {
            var conversation = await _ctx.Conversations
                .Include(c => c.Admin)
                .Include(c => c.Organizer)
                .FirstOrDefaultAsync(c => c.AdminId == adminId && c.OrganizerId == organizerId);

            if (conversation == null) return null;

            return new ConversationDto
            {
                Id = conversation.Id,
                AdminId = conversation.AdminId,
                AdminUsername = conversation.Admin?.Username,
                OrganizerId = conversation.OrganizerId,
                OrganizerUsername = conversation.Organizer?.Username,
                Created = conversation.Created
            };
        }

        public async Task<MessageDto> SendMessageAsync(SendMessageDto dto)
        {
            var message = new Message
            {
                ConversationId = dto.ConversationId,
                SenderId = dto.SenderId,
                MessageText = dto.MessageText,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _ctx.Messages.Add(message);
            await _ctx.SaveChangesAsync();

            var user = await _ctx.Users.FindAsync(dto.SenderId);


            var messageDto = new MessageDto
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                SenderUsername = user?.Username ?? "Unknown",
                MessageText = message.MessageText,
                ImageUrl = message.ImageUrl,
                CreatedAt = message.CreatedAt,
                IsRead = message.IsRead
            };

            await _hubContext.Clients.Group(dto.ConversationId.ToString())
                .SendAsync("ReceiveMessage", messageDto);

            return messageDto;
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _ctx.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _ctx.SaveChangesAsync();
            }
        }

        public Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId)
        {
           var conversations = _ctx.Conversations
                .Include(c => c.Messages)
                .Include(c => c.Admin)
                .Include(c => c.Organizer)
                .Where(c => c.AdminId == userId || c.OrganizerId == userId)
                .OrderByDescending(c => c.Messages.Max(m => (DateTime?)m.CreatedAt) ?? c.Created)
                .Select(c => new ConversationDto
                {
                    Id = c.Id,
                    AdminId = c.AdminId,
                    AdminUsername = c.Admin.Username,
                    OrganizerId = c.OrganizerId,
                    OrganizerUsername = c.Organizer.Username,
                    Created = c.Created
                });
            return conversations.ToListAsync();
        }

        public async Task<ConversationDto?> GetConversationByIdAsync(int conversationId, Guid currentUserId)
        {
            // Get the conversation first
            var conv = await _ctx.Conversations
                .Include(c => c.Admin)
                .Include(c => c.Organizer)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conv == null) return null;

            // Validate that the current user is either the admin or the organizer
            if (conv.AdminId != currentUserId && conv.OrganizerId != currentUserId)
                return null;

            // Join messages with users to get the sender username
            var messages = await _ctx.Messages
                .Where(m => m.ConversationId == conversationId)
                .Join(_ctx.Users,
                      m => m.SenderId,
                      u => u.Id,
                      (m, u) => new MessageDto
                      {
                          Id = m.Id,
                          ConversationId = m.ConversationId,
                          SenderId = m.SenderId,
                          SenderUsername = u.Username,
                          MessageText = m.MessageText,
                          ImageUrl = m.ImageUrl,
                          CreatedAt = m.CreatedAt,
                          IsRead = m.IsRead
                      })
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return new ConversationDto
            {
                Id = conv.Id,
                AdminId = conv.AdminId,
                AdminUsername = conv.Admin?.Username ?? "Unknown",
                OrganizerId = conv.OrganizerId,
                OrganizerUsername = conv.Organizer?.Username ?? "Unknown",
                Created = conv.Created,
                Messages = messages
            };
        }
    }
}

