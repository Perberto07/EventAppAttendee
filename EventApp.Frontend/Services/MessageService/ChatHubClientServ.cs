using EventApp.Shared.DTOs.Message;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;


namespace EventApp.Frontend.Services.MessageService
{
    public class ChatHubClientServ
    {
        private readonly HubConnection _hubConnection;

        public event Action<MessageDto>? OnMessageReceived;

        public ChatHubClientServ(NavigationManager navigationManager, JwtAuthenticationStateProvider authProvider)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7216/chathub" , options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        return await authProvider.GetRawTokenAsync();
                    };
                })
                .Build();

            _hubConnection.On<MessageDto>("ReceiveMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });
        }

        public async Task StartAsync() => await _hubConnection.StartAsync();

        public async Task JoinConversation(int conversationId) =>
            await _hubConnection.InvokeAsync("JoinConversation", conversationId);

        public async Task SendMessage(MessageDto message) =>
             await _hubConnection.InvokeAsync("SendMessage", message);
    }
}
