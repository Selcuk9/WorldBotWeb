using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.ViewsModels
{
    public class InstaUserModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        public string Passsword { get; set; }

    }
}
