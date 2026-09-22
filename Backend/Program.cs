using Backend.Repositories;
using Backend.Services;
using CloudinaryDotNet;
using Dapper;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<FactoryRepository>();
builder.Services.AddScoped<SerieRepository>();
builder.Services.AddScoped<SerieService>();
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<EpisodioRepository>();
builder.Services.AddScoped<EpisodioService>();
DotEnv.Load();

var account = new Account(
    Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
);

builder.Services.AddSingleton(new Cloudinary(account));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

app.MapControllers();

app.Run();
