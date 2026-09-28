using GestionAula.Data.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ EF Core
builder.Services.AddDbContext<GestionAulaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionAulaConnection")));

// ✅ Controllers
builder.Services.AddControllers();

// ✅ OpenAPI
builder.Services.AddOpenApi();

// ✅ CORS: en desarrollo permitimos cualquier origen (evita problemas de puertos)
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("BlazorPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();