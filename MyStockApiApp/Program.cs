using Microsoft.EntityFrameworkCore;
using MyStockApi.Data;

var builder = WebApplication.CreateBuilder(args);
// --- เพิ่มตรงนี้ครับ ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxt", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL ของ Nuxt 4
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// ----------------------

builder.Services.AddControllers();
// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// --- เพิ่มตรงนี้ครับ (ต้องวางก่อน app.MapControllers()) ---
app.UseCors("AllowNuxt");
// -----------------------------------------------------

app.UseAuthorization();

app.MapControllers();

app.Run();
