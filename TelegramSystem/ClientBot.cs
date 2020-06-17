using MihaZupan;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSystem
{
    public class ClientBot
    {
        private readonly string token;
        private ITelegramBotClient botClient;
        public ClientBot(string token)
        {
            if (!string.IsNullOrEmpty(token) || !string.IsNullOrWhiteSpace(token))
            {
                this.token = token;
            }
            
        }
        public User GetBot()
        {
            if (token != null)
            {
                var proxy = new HttpToSocks5Proxy("94.103.81.38", 1088);
                proxy.ResolveHostnamesLocally = true;
                botClient = new TelegramBotClient(token,proxy);
                var botTelegram = botClient.GetMeAsync().Result;
                return botTelegram;
            }
            return new User();
        }
    }
}
