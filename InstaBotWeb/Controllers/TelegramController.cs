using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TelegramSystem;

namespace InstaBotWeb.Controllers
{
    public class TelegramController : Controller
    {
        [HttpGet]
        public  IActionResult TelegramControl()
        {
            return View("Telegram");
        }

        [HttpPost]
        public async Task<string> GetBot([FromBody]string token)
        {
            var botToken = token;
            ClientBot clientBot = new ClientBot(botToken);
            var bot = JsonConvert.SerializeObject(clientBot.GetBot());
            return bot;
        }
    }
}
