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

// �������� ����� ���� �� ������������
var telegramBotToken = builder.Configuration["TelegramBotToken"];

// ��������� ������ Telegram ����
builder.Services.AddSingleton<TelegramBotService>(sp => new TelegramBotService(telegramBotToken));

// ������������ FileService
builder.Services.AddHttpClient<FileService>(client =>
{
    client.BaseAddress = new Uri($"https://api.telegram.org/bot{telegramBotToken}/");
});

// ��������� Razor ����������
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
await telegramBotService.StartAsync(); // ������ ������������ ������

app.Run();

////
///
