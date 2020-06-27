using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.ViewsModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress]
        [UIHint("email")]
        public string Email { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
