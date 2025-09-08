using EventApp.Shared.DTOs.Message;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(MessageDto message)
    {
        var groupName = $"conversation-{message.ConversationId}";
        await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinConversation(int conversationId)
    {
        var groupName = $"conversation-{conversationId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveConversation(int conversationId)
    {
        var groupName = $"conversation-{conversationId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
