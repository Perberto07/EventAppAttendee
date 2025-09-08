using EventApp.Shared.DTOs.Seat;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventApp.Frontend.Services.SeatService
{
    public class SeatSignalRService
    {
        private HubConnection _hub;
        private Guid _currentEventId;

        public event Action<SeatUpdateDto> OnSeatUpdated;

        public async Task StartAsync(string hubBaseUrl, Guid eventId, Func<Task<string?>>? accessTokenProvider = null)
        {
            ArgumentNullException.ThrowIfNull(hubBaseUrl);
            _currentEventId = eventId;

            try
            {
                var builder = new HubConnectionBuilder();

                if (accessTokenProvider is null)
                {
                    _hub = builder
                        .WithUrl($"https://localhost:7103/hubs/seat") // use hubBaseUrl parameter
                        .WithAutomaticReconnect()
                        .Build();
                }
                else
                {
                    _hub = builder
                        .WithUrl($"https://localhost:7103/hubs/seat", options =>
                        {
                            options.AccessTokenProvider = accessTokenProvider; // if hub is protected
                        })
                        .WithAutomaticReconnect()
                        .Build();
                }

                _hub.On<SeatUpdateDto>("SeatUpdated", update =>
                {
                    OnSeatUpdated?.Invoke(update);
                });

                _hub.Reconnected += async _ =>
                {
                    if (_hub?.State == HubConnectionState.Connected)
                    {
                        await _hub.SendAsync("JoinEventGroup", _currentEventId);
                    }
                };

                // Try starting the connection
                await _hub.StartAsync();

                // Join the event group once connected
                await _hub.SendAsync("JoinEventGroup", eventId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"⚠️ Failed to connect to SignalR hub at {hubBaseUrl}: {ex.Message}");
            }
        }


        public async Task StopAsync()
        {
            if (_hub is null) return;

            try
            {
                if (_hub.State == HubConnectionState.Connected && _currentEventId != Guid.Empty)
                {
                    await _hub.SendAsync("LeaveEventGroup", _currentEventId);
                }
            }
            finally
            {
                await _hub.StopAsync();
                await _hub.DisposeAsync();
                _hub = null;
                _currentEventId = Guid.Empty;
            }
        }

        public async ValueTask DisposeAsync() => await StopAsync();
    }
}
