using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaBotWeb.Models.Telegram
{
    public class UserTelegram
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int BotId { get; set; }
        public TelegramBot TelegramBot { get; set; }
    }
}
