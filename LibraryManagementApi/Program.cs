using LibraryManagementApi.Data;
using LibraryManagementApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Servis Kayýtlarý
builder.Services.AddControllers();

// EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2) Global Error Handling Middleware
app.UseGlobalExceptionHandler();

// 3) Swagger Middleware (yalnýzca Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManagement API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 4) Controller Endpoint’leri
app.MapControllers();

app.Run();