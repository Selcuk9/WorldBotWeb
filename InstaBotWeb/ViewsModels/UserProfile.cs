using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.ViewsModels
{
    public class UserProfile
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile Avatar { get; set; }
        public string AvatarPath { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
