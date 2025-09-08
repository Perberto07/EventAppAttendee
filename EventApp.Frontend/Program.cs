using Blazored.LocalStorage;

using EventApp.Frontend;
using EventApp.Frontend.Services.Auth;
using EventApp.Frontend.Services.ClientLocationService;
using EventApp.Frontend.Services.ClientTicket;
using EventApp.Frontend.Services.ClientTransactionServ;
using EventApp.Frontend.Services.Event;
using EventApp.Frontend.Services.EventSeatService;
using EventApp.Frontend.Services.LayoutClient;
using EventApp.Frontend.Services.LoadingServ;
using EventApp.Frontend.Services.MessageService;
using EventApp.Frontend.Services.NewClientLocationService;
using EventApp.Frontend.Services.NewEventServ;
using EventApp.Frontend.Services.PhilippineTimeService;
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
builder.Services.AddScoped<ITicketClientService, TicketClientService>();
builder.Services.AddSingleton<LoadingService>();
builder.Services.AddScoped<IEventSeatService, EventSeatService>();
builder.Services.AddScoped<IClientLocationServ, ClientLocationServ>();
builder.Services.AddScoped<IClientTransactionService, ClientTransactionService>();
builder.Services.AddScoped<IClientMessageServ, ClientMessageServ>();
builder.Services.AddScoped<ILayoutClientServ, LayoutClientServ>();
builder.Services.AddScoped<IClientLocationService, ClientLocationService>();
builder.Services.AddScoped<IClientEventServ, ClientEventServ>();
builder.Services.AddScoped<ChatHubClientServ>();
builder.Services.AddScoped<IPhTimeServ, PhTimeServ>();
builder.Services.AddMudServices();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
