using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using BlazorApp1.Classes;
using Newtonsoft.Json;

namespace BlazorApp1.Services
{
    // TelegramBotService class
    public class TelegramBotService
    {
        private readonly TelegramBotClient _botClient;

        public TelegramBotService(string token)
        {
            _botClient = new TelegramBotClient(token);
        }

        public async Task StartAsync()
        {
            // Настройки получения обновлений
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: default);

            var botMe = await _botClient.GetMeAsync();
            Console.WriteLine($"Запущен бот @{botMe.Username}");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { Text: { } } message)
            {
                Console.WriteLine($"Получено сообщение: {message.Text}");

                var keyboard = new InlineKeyboardMarkup(new[]
                {
                InlineKeyboardButton.WithCallbackData("Start", "start"),
                InlineKeyboardButton.WithCallbackData("Добавить файл", "add_file")
            });

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Выберите действие:",
                    replyMarkup: keyboard,
                    cancellationToken: cancellationToken
                );
            }
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                var callbackQuery = update.CallbackQuery;

                if (callbackQuery.Data == "start")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.Message.Chat.Id,
                        text: "Вы начали работу",
                        cancellationToken: cancellationToken
                    );
                }
                else if (callbackQuery.Data == "add_file")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.Message.Chat.Id,
                        text: "Пожалуйста, загрузите файл",
                        cancellationToken: cancellationToken
                    );
                }
            }
            else if (update.Message is { Document: { } } fileMessage)
            {
                var fileId = fileMessage.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId, cancellationToken);

                Console.WriteLine($"Получен файл: {fileMessage.Document.FileName}");

                await botClient.SendTextMessageAsync(
                    chatId: fileMessage.Chat.Id,
                    text: $"Файл {fileMessage.Document.FileName} успешно получен.",
                    cancellationToken: cancellationToken
                );
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Ошибка Telegram API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }

    // FileService class
    public class FileService
    {
        private readonly HttpClient _httpClient;

        public FileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FileItem>> GetFilesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("getUpdates");
                Console.WriteLine($"Ответ от Telegram API: {response}");

                var updatesResponse = JsonConvert.DeserializeObject<UpdatesResponse>(response);

                if (updatesResponse?.Result == null || updatesResponse.Result.Count == 0)
                {
                    Console.WriteLine("Нет обновлений с файлами");
                    return new List<FileItem>();
                }

                var files = updatesResponse.Result
                    .Where(update => update.Message?.Document != null)
                    .Select(update => new FileItem
                    {
                        Name = update.Message.Document?.FileName ?? "Unknown file",
                        Url = $"https://api.telegram.org/file/bot{_httpClient.BaseAddress.Segments[2]}/{update.Message.Document.FileId}"
                    })
                    .ToList();

                Console.WriteLine($"Файлы: {files.Count}");
                return files;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении файлов: {ex.Message}");
                return new List<FileItem>();
            }
        }

    }
}
