using InstaBotWeb.Models.Telegram;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string AvatarProfile { get; set; }

        public List<UserTelegram> UserTelegrams { get; set; } = new List<UserTelegram>();
    }
}
