using Blazored.LocalStorage;
using EventApp.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventApp.Frontend.Services.ClientTransactionServ
{
    public class ClientTransactionService : IClientTransactionService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ClientTransactionService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        // ✅ Get transactions for the logged-in user
        public async Task<List<EventTransaction>> GetMyTransactionsAsync()
        {
            await AttachTokenAsync();
            var response = await _http.GetFromJsonAsync<List<EventTransaction>>("api/transactions/my-transactions");
            return response ?? new List<EventTransaction>();
        }

        // ✅ Get all transactions (Admin only)
        public async Task<List<EventTransaction>> GetAllTransactionsAsync()
        {
            await AttachTokenAsync();
            var response = await _http.GetFromJsonAsync<List<EventTransaction>>("api/transactions/all");
            return response ?? new List<EventTransaction>();
        }

        private async Task AttachTokenAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
