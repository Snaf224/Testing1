using BlazorApp1.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using BlazorApp1.Services;
using BlazorApp1.Classes;

var builder = WebApplication.CreateBuilder(args);

// Получаем токен бота из конфигурации
var telegramBotToken = builder.Configuration["TelegramBotToken"];

// Добавляем сервис Telegram бота
builder.Services.AddSingleton<TelegramBotService>(sp => new TelegramBotService(telegramBotToken));

// Регистрируем FileService
builder.Services.AddHttpClient<FileService>(client =>
{
    client.BaseAddress = new Uri($"https://api.telegram.org/bot{telegramBotToken}/");
});

// Добавляем Razor компоненты
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var telegramBotService = app.Services.GetRequiredService<TelegramBotService>();
await telegramBotService.StartAsync(); // Запуск асинхронного метода

app.Run();

////
///
