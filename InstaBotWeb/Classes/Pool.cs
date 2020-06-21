using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaBotWeb.Classes
{
    //CancellationTokenSource
    public sealed class Pool
    {
        private static readonly Lazy<Pool> instance =
            new Lazy<Pool>(()=> new Pool());
        public static Pool Instance { get { return instance.Value; } }
        public Queue<CancellationTokenSource> TokePools { get; set; }
        public Queue<ITelegramBotClient> ClientPools { get; set; }
        private Pool()
        {
            TokePools = new Queue<CancellationTokenSource>();
            ClientPools = new Queue<ITelegramBotClient>();
        }

    }
}
