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
    [Authorize]
    public class TelegramController : Controller
    {
        private readonly ApplicationContext dbContext;
        private ClientBot clientBot;
        private readonly Pool threads;
        public TelegramController(ApplicationContext db, Pool threads)
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
            var  tokenBot = token;
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
       
        [HttpPost]
        public void RunBot()
        {
            var tokenBots = GetBots();
            foreach (var item in tokenBots)
            {
                clientBot = new ClientBot(item.TokenBot);
                clientBot.GetBot();
                clientBot.RunBotAsync(true);

                var cancellationTokenSource = clientBot.tokenPools.Dequeue();
                threads.ClientPools.Enqueue(clientBot.botClient);
                threads.TokePools.Enqueue(cancellationTokenSource);
            }
        }
        [HttpPost]
        public void StopBot()
        {
            for (int i = 0; i < threads.TokePools.Count; i++)
            {
                threads.TokePools.Peek().Cancel();
                threads.ClientPools.Peek().StopReceiving();
            }
        }
        
    }
}
