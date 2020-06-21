using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InstaBotWeb.Models.Telegram
{
    public class TelegramBot
    {
        public int TokenId { get; set; }
        public string Token { get; set; }

        public string UsernameBots { get; set; }

        public List<UserTelegram> UserTelegrams { get; set; } = new List<UserTelegram>();
    }
}
