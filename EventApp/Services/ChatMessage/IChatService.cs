using EventApp.Shared.DTOs.Message;
using EventApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Services.ChatMessage
{
    public interface IChatService
    {
        Task<ConversationDto?> GetConversationAsync(Guid adminId, Guid organizerId);
        Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId);
        Task<ConversationDto> CreateConversationAsync(CreateConversationDto dto);
        Task<MessageDto> SendMessageAsync(SendMessageDto dto);
        Task MarkMessageAsReadAsync(int messageId);
        Task <ConversationDto?> GetConversationByIdAsync(int conversationId, Guid currentUserid);
    }
}
