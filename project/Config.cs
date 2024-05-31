using Newtonsoft.Json;

namespace ShibaBot
{
    class Config
    {

        [JsonProperty("TelegramBotToken")]
        public string TelegramBotToken { get; set; }
        
        public static Config LoadConfig(string path)
        {
            Console.WriteLine("FreeToGame Bot (Telegram)");
            Console.WriteLine();
            try
            {
                string json = File.ReadAllText(path);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("> Файл JSON (ваш ключ):");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(json);
                Console.WriteLine();
                return JsonConvert.DeserializeObject<Config>(json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Не вдалось завантажити токен телеграм бота: " + ex.Message);
                return null;
            }
        }
    }
}
