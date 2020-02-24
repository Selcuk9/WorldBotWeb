using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.Models
{
    public class UserInstaAndUser
    {
        public long UserId { get; set; }
        public long InstaUserId { get; set; }
    }
}
