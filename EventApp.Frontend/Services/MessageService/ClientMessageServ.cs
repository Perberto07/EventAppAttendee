using EventApp.Shared.DTOs.Message;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.MessageService
{
    public class ClientMessageServ: IClientMessageServ
    {
        private readonly HttpClient _http;

        public ClientMessageServ(HttpClient http)
        {
            _http = http;
        }

        // Get or create a conversation (backend ensures uniqueness)
        public async Task<ConversationDto?> GetOrCreateConversationAsync(Guid adminId, Guid organizerId)
        {
            var dto = new CreateConversationDto
            {
                AdminId = adminId,
                OrganizerId = organizerId
            };

            var response = await _http.PostAsJsonAsync("api/chatmessage/conversation", dto);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ConversationDto>();
        }

        // Get all conversations of the current user (backend uses JWT for user)
        public async Task<List<ConversationDto>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _http.GetFromJsonAsync<List<ConversationDto>>($"api/chatmessage/my-conversations/{userId}");
            return conversations ?? new List<ConversationDto>();
        }

        // Get conversation by ID (backend validates current user)
        public async Task<ConversationDto?> GetConversationByIdAsync(int conversationId, Guid userId)
        {
            var conversation = await _http.GetFromJsonAsync<ConversationDto>($"api/chatmessage/conversation/{conversationId}/{userId}");
            return conversation;
        }

        // Get all messages in a conversation with another user
        public async Task<List<MessageDto>> GetMessagesAsync(Guid otherUserId)
        {
            var messages = await _http.GetFromJsonAsync<List<MessageDto>>($"api/chatmessage/conversation/{otherUserId}");
            return messages ?? new List<MessageDto>();
        }

        // Send a new message
        public async Task<MessageDto> SendMessageAsync(SendMessageDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/chatmessage/send", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MessageDto>()
                   ?? throw new Exception("Failed to send message.");
        }

        // Mark a message as read
        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var response = await _http.PostAsync($"api/chatmessage/read/{messageId}", null);
            response.EnsureSuccessStatusCode();
        }

        // Upload image for chat messages
        public async Task<string?> UploadImageAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var stream = file.OpenReadStream();
            content.Add(new StreamContent(stream), "file", file.Name);

            var response = await _http.PostAsync("api/chatmessage/upload", content);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<string>();
        }
    }
}
