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
        public string Token { get; }
        public ITelegramBotClient BotClient { get; }
        public Queue<CancellationTokenSource> TokenPools { get;}
        public ClientBot(){}
        public ClientBot(string token)
        {
            if (!string.IsNullOrEmpty(token) || !string.IsNullOrWhiteSpace(token))
            {
                TokenPools = new Queue<CancellationTokenSource>();
                this.Token = token;
                BotClient = new TelegramBotClient(token);
            }
        }
        public User GetBot()
        {
            if (Token != null)
            {
                var botTelegram = BotClient.GetMeAsync().Result;
                return botTelegram;
            }
            return new User();
        }
        public void DeleteBot(string token)
        {

        }
        public void RunBotAsync(bool runBot)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            TokenPools.Enqueue(cancelTokenSource);
            BotClient.OnMessage += Bot_OnMessage;
            BotClient.StartReceiving();
            
            if (!runBot)
            {
                TokenPools.FirstOrDefault().Cancel();
                BotClient.StopReceiving();
                return;
            }
           Task.Run(() => Thread.Sleep(int.MaxValue));
        }
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if(e.Message.Text != null)
            {
                await BotClient.SendTextMessageAsync(
                   chatId: e.Message.Chat,
                   text: "You said\n" + e.Message.Text
                   );
            }

        }
    }
}
