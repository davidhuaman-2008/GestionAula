using GestionAula.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ✅ HttpClient hacia el API
builder.Services.AddHttpClient("GestionAulaApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5128/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// ✅ Servicio tipado
builder.Services.AddScoped<GestionAula.Web.Services.ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();