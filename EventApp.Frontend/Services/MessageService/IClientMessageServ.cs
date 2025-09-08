using EventApp.Shared.DTOs.Message;

namespace EventApp.Frontend.Services.MessageService
{
    public interface IClientMessageServ
    {
        Task<ConversationDto?> GetOrCreateConversationAsync(Guid adminId, Guid organizerId);
        Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId);
        Task<ConversationDto?> GetConversationByIdAsync(int conversationId, Guid userId);
        Task<List<MessageDto>> GetMessagesAsync(Guid otherUserId);
        Task<MessageDto> SendMessageAsync(SendMessageDto dto);
        Task MarkMessageAsReadAsync(int messageId);
        Task<string?> UploadImageAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file);
    }
}
