using BlazorApp.Components;
using BlazorApp.Services;
using Google.Cloud.Firestore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register Firestore services
builder.Services.AddSingleton<FirestoreService>();
builder.Services.AddScoped<StudentService>();

// If using environment variables for configuration
builder.Services.Configure<FirebaseConfig>(options =>
{
    options.ProjectId = builder.Configuration["FIREBASE_PROJECT_ID"] 
                        ?? builder.Configuration["Firebase:ProjectId"] 
                        ?? throw new InvalidOperationException("Firebase ProjectId is not configured");
    
    options.ApiKey = builder.Configuration["FIREBASE_API_KEY"] 
                     ?? builder.Configuration["Firebase:ApiKey"] 
                     ?? throw new InvalidOperationException("Firebase ApiKey is not configured");
    
    options.AuthDomain = builder.Configuration["FIREBASE_AUTH_DOMAIN"] 
                         ?? builder.Configuration["Firebase:AuthDomain"] 
                         ?? string.Empty;
    
    options.StorageBucket = builder.Configuration["FIREBASE_STORAGE_BUCKET"] 
                            ?? builder.Configuration["Firebase:StorageBucket"] 
                            ?? string.Empty;
});
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
