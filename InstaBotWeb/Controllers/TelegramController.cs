using InstaBotWeb.Classes;
using InstaBotWeb.CookiesUser;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.Models.Telegram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramSystem;

namespace InstaBotWeb.Controllers
{
    /// <summary>
    /// Данный коннтроллер для работы с телеграм ботом
    /// </summary>
    [Authorize]
    public class TelegramController : Controller
    {
        private readonly ApplicationContext dbContext;
        private ClientBot clientBot;
        private readonly Dictionary<int,List<Pool>> threads;
        public TelegramController(ApplicationContext db, Dictionary<int, List<Pool>> threads)
        {
            this.threads = threads;
            dbContext = db;
        }
        [HttpGet]
        public IActionResult TelegramControl()
        {
            var botsTelega = GetBots();
            return View("Telegram",botsTelega);
        }
        /// <summary>
        /// Возврашаем список токенов ботов телеграм
        /// </summary>
        /// <returns></returns>
        private List<DataBot> GetBots()
        {
            var userId = ClaimUser.GetIdUser(User);
                var bots = (from Tb in dbContext.TelegramBots
                        join Ut in dbContext.UserTelegrams on Tb.TokenId equals Ut.BotId
                        where Ut.UserId == userId
                        select new DataBot { TokenBot = Tb.Token, UsernameBot = Tb.UsernameBots }
                        ).ToList();
            return bots;
        }
        /// <summary>
        /// Добавляет бота в базу данных
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddBot([FromBody] string token)
        {
            string botJson = "Бот не найден";
            User bot;
            var tokenBot = token;
            try
            {
                clientBot = new ClientBot(tokenBot);
                bot = clientBot.GetBot();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var idUser = ClaimUser.GetIdUser(User);
            if (idUser != -1 && bot.IsBot)
            {
                var botik = new TelegramBot { Token = token, UsernameBots = bot.Username };
                await dbContext.TelegramBots.AddAsync(botik);
                await dbContext.SaveChangesAsync();
                botik.UserTelegrams.Add(new UserTelegram { UserId = idUser, BotId = botik.TokenId });
                dbContext.SaveChanges();
                botJson = JsonConvert.SerializeObject(bot);
            }
            return botJson;
        }
       /// <summary>
       /// Запускаем бота и добавляем токен задач в cписок с ключом
       /// Каждый пользователь имеет свои задачи
       /// </summary>
        [HttpPost]
        public void RunBots()
        {
            var tokenBots = GetBots();
            foreach (var token in tokenBots)
            {
                RegisterBot(token.TokenBot);
            }
        }

        [HttpPost]
        public void RunBot([FromBody]string token)
        {
            RegisterBot(token);
        }

        private void RegisterBot(string token)
        {
            var pool = Pool.Instance;
            var tokenBot = token;
            var idUser = ClaimUser.GetIdUser(User);

            //запускаем и открываем задачу
            clientBot = new ClientBot(tokenBot);
            clientBot.GetBot();
            clientBot.RunBotAsync(true);

            //регистрация потока
            var cancellationTokenSource = clientBot.TokenPools.Dequeue();
            pool.ClientPools.Enqueue(clientBot.BotClient);
            pool.TokePools.Enqueue(cancellationTokenSource);

            //к ид юзера привязываем список его задач(или потоки)
            if (!threads.ContainsKey(idUser))
            {
                threads.Add(idUser, new List<Pool>());
            }
            threads[idUser].Add(pool);
            
        }


        [HttpPost]
        public void StopBots()
        {
            var idUser = ClaimUser.GetIdUser(User);
            if (threads.ContainsKey(idUser))
            {
                var myThreads = threads[idUser];
                foreach (var pool in myThreads)
                {
                    pool.TokePools.Dequeue().Cancel();
                    pool.ClientPools.Dequeue().StopReceiving();
                }
                threads.Remove(idUser);
            }
        }

        /// <summary>
        /// Ужас, надо это метод уменьшить
        /// TODO nigga
        /// </summary>
        /// <param name="token"></param>
        [HttpPost]
        public void StopBot([FromBody]string token)
        {
            if (token != null)
            {
                var delBot = new ClientBot(token).GetBot();
                var idUser = ClaimUser.GetIdUser(User);
                var Mythreads = threads[idUser];
                if (threads.ContainsKey(idUser))
                {
                    
                    foreach (var pool in Mythreads)
                    {
                        var currentBot = pool.ClientPools.Peek().BotId;
                        if (currentBot == delBot.Id)
                        {
                            pool.TokePools.Dequeue().Cancel();
                            pool.ClientPools.Dequeue().StopReceiving();
                        }
                    }

                } 
            }
        }
        
    }
}
