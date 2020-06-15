using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InstaBotWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace InstaBotWeb.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AOuth", "Account");
            }
            else
            {
                return View();
            }
        }
    }
}
