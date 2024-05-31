using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Newtonsoft.Json;

namespace ShibaBot
{
    public static class BotActions
    {
        public static async Task StartAsync(ITelegramBotClient botClient, long chatId, ReplyKeyboardMarkup keyboardMarkup, CancellationToken token)
        {
            await botClient.SendPhotoAsync(chatId,
                photo: InputFile.FromUri("https://digiday.com/wp-content/uploads/sites/3/2022/11/gaming-reality.jpeg?w=1030&h=579&crop=1"),
                cancellationToken: token);
            await botClient.SendTextMessageAsync(chatId, 
                text: "Hi! This is FreeToGame NonOfficial Telegram Bot!", 
                parseMode: ParseMode.Html);
            await botClient.SendTextMessageAsync(chatId, 
                text: "👇<b>Select action</b>", 
                replyMarkup: keyboardMarkup, 
                cancellationToken: token, 
                parseMode: ParseMode.Html);
        }

        public static async Task AboutAsync(ITelegramBotClient botClient, long chatId, CancellationToken token)
        {
            await botClient.SendTextMessageAsync(chatId, 
                text: "❓<b>Бот створений під час виконання практичної роботи №2 з АППЗ .Net</b>\n\nБот використовує:\n- Telegram.Bot,\n- Visual Studio 2022,\n- https://freetogame.com/ у якості API.", 
                parseMode: ParseMode.Html);
        }

        public static async Task DefaultAsync(ITelegramBotClient botClient, long chatId, ReplyKeyboardMarkup keyboardMarkup, CancellationToken token)
        {
            await botClient.SendTextMessageAsync(chatId, 
                text: "Don't understand you :( \n\n👇<b>Select action</b>", 
                replyMarkup: keyboardMarkup, 
                cancellationToken: token, 
                parseMode: ParseMode.Html);
        }

        public static async Task GetGameListAsync(ITelegramBotClient botClient, long chatId, CancellationToken token)
        {
            string baseUrl = "https://www.freetogame.com/api/games?platform=pc";
            string apiUrl = baseUrl;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        dynamic gamesData = JsonConvert.DeserializeObject(jsonResult);

                        if (gamesData.Count > 0)
                        {
                            Random random = new Random();
                            int randomIndex = random.Next(0, gamesData.Count);
                            var firstGame = gamesData[randomIndex];
                            string title = firstGame.title;
                            string thumbnailUrl = firstGame.thumbnail;
                            string shortDescription = firstGame.short_description;
                            string gameUrl = firstGame.game_url;
                            string genre = firstGame.genre;
                            string platform = firstGame.platform;
                            string publisher = firstGame.publisher;
                            string developer = firstGame.developer;
                            string releaseDate = firstGame.release_date;
                            string profileUrl = firstGame.freetogame_profile_url;

                            string message = $"<b>{title}</b>\nGenre: {genre}\nPlatform: {platform}\nPublisher: {publisher}\nDeveloper: {developer}\nRelease Date: {releaseDate}\nShort Description: {shortDescription}\n<a href='{thumbnailUrl}'>Thumbnail</a>\n<a href='{gameUrl}'>Game URL</a>\n<a href='{profileUrl}'>Profile URL</a>";

                            await botClient.SendTextMessageAsync(chatId,
                                text: message,
                                cancellationToken: token,
                                parseMode: ParseMode.Html);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId,
                                text: "No games found.",
                                cancellationToken: token,
                                parseMode: ParseMode.Html);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error fetching data: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


    }
}
