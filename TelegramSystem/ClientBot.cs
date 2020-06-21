using MihaZupan;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramSystem
{
    public class ClientBot
    {
        private readonly string token;
        public ITelegramBotClient botClient;
        public Queue<CancellationTokenSource> tokenPools;
        public ClientBot(){}
        public ClientBot(string token)
        {
            if (!string.IsNullOrEmpty(token) || !string.IsNullOrWhiteSpace(token))
            {
                tokenPools = new Queue<CancellationTokenSource>();
                this.token = token;
                //var proxy = new HttpToSocks5Proxy("184.178.172.5", 15303);
                //proxy.ResolveHostnamesLocally = true;
                // Отмена запрета телеграм))
                botClient = new TelegramBotClient(token);
            }
        }
        public User GetBot()
        {
            if (token != null)
            {
                var botTelegram = botClient.GetMeAsync().Result;
                return botTelegram;
            }
            return new User();
        }
        public void RunBotAsync(bool runBot)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            tokenPools.Enqueue(cancelTokenSource);
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            
            if (!runBot)
            {
                tokenPools.FirstOrDefault().Cancel();
                botClient.StopReceiving();
                return;
            }
           Task.Run(() => Thread.Sleep(int.MaxValue));
        }
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {

            if(e.Message.Text != null)
            {
                await botClient.SendTextMessageAsync(
                   chatId: e.Message.Chat,
                   text: "You said\n" + e.Message.Text
                   );
            }

        }
    }
}
