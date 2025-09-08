using EventApp.Shared.DTOs.Auth;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using EventApp.Shared.DTOs.Auth.OTPRegister;

namespace EventApp.Frontend.Services.Auth
{
    public class ClientService : IClientService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authStateProvider;

        private const string TokenKey = "authToken";

        public ClientService(HttpClient http, IJSRuntime js, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _js = js;
            _authStateProvider = authStateProvider;
        }

        public async Task<LoginResultDto?> LoginAsync(LoginRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                // Optionally, read error response
                var error = await response.Content.ReadAsStringAsync();
                return new LoginResultDto { Success = false, ErrorMessage = error };
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();

            if (result != null && result.Success && !string.IsNullOrEmpty(result.Token))
            {
                await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

                if (_authStateProvider is JwtAuthenticationStateProvider jwtProvider)
                    jwtProvider.NotifyUserAuthentication(result.Token);
            }

            return result;
        }

        public async Task<string?> StartRegistrationAsync(string email)
        {
            var response = await _http.PostAsJsonAsync("api/auth/start-registration", new { Email = email });
            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? content
                : content;
        }

        public async Task<string?> CompleteRegistrationAsync(CompleteRegistrationDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/complete-registration", dto);

            if (!response.IsSuccessStatusCode) return null;

            var token = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            if (_authStateProvider is JwtAuthenticationStateProvider jwtProvider)
                jwtProvider.NotifyUserAuthentication(token);

            return token;
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _http.DefaultRequestHeaders.Authorization = null;

            if (_authStateProvider is JwtAuthenticationStateProvider jwtProvider)
                await jwtProvider.NotifyUserLogout();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }

        public bool IsTokenExpired(string token)
        {
            var payload = token.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null && keyValuePairs.TryGetValue("exp", out var expValue))
            {
                var exp = Convert.ToInt64(expValue);
                var expTime = DateTimeOffset.FromUnixTimeSeconds(exp);
                return expTime <= DateTimeOffset.UtcNow;
            }

            return true;
        }

        public async Task EnsureTokenValidAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return;

            if (IsTokenExpired(token))
            {
                await LogoutAsync();
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
