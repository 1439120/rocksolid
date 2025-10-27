using BlazorApp.Authentication;
using BlazorApp.Components;
using BlazorApp.Services;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Blazor authentication
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();


// Register FirestoreService
builder.Services.AddSingleton<FirestoreService>();
builder.Services.AddScoped<FirebaseAuthentication>(); // Firebase authentication injection
// storage and auth rpovider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
// builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddMemoryCache();

// other services
builder.Services.AddScoped<StudentService>(); // Your other services
builder.Services.AddScoped<ClientDatabaseService>(); // Your other services
builder.Services.AddScoped<ApplicationService>(); // Your other services

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Middleware for a bunch of things
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Configuration class
public class FirebaseConfig
{
    public string ProjectId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string AuthDomain { get; set; } = string.Empty;
    public string StorageBucket { get; set; } = string.Empty;
}
