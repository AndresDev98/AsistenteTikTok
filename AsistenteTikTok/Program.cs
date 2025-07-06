using AsistenteTikTok.Application.Interfaces;
using AsistenteTikTok.Application.UseCases;
using AsistenteTikTok.Infrastructure.Adb;
using AsistenteTikTok.Infrastructure.Email;
using AsistenteTikTok.Infrastructure.Files;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias
builder.Services.AddSingleton<IEmailService, SecMailEmailService>();
builder.Services.AddSingleton<IAdbService, AdbService>();
builder.Services.AddSingleton<IBotAccountRepository, FileBotAccountRepository>();
builder.Services.AddScoped<BotCreationService>();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
