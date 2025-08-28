using Blazored.LocalStorage;
using BookEvent.Attendee.Frontend.Services;
using EventApp.Client.Services.SeatService;
using EventApp.Frontend;
using EventApp.Frontend.Services.Auth;
using EventApp.Frontend.Services.Event;
using EventApp.Frontend.Services.SeatLayoutClient;
using EventApp.Frontend.Services.SeatService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<SeatSignalRService>();


builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7216/") 
});

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISeatClientService, SeatClientService>();
builder.Services.AddScoped<ISeatLayoutClient, SeatLayoutClient>();
builder.Services.AddMudServices();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
