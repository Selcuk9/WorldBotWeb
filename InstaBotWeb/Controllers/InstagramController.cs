using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstaBotWeb.Models;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InstaBotWeb.Controllers
{
    public class InstagramController : Controller
    {
        private readonly ApplicationContext db;
        User user;
        public InstagramController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult InstagramAdding()
        {
            return View("AddInst");
        }

        [HttpPost]
        public async Task<IActionResult> InstagramAdding(InstaUserModel model)
        {
           user= await db.DbUser.FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            if (ModelState.IsValid)
            {
                var check = await db.DbInstaUser.FirstOrDefaultAsync(e => e.UserName == model.UserName); // TODO
                if (check == null)
                {
                    db.DbInstaUser.Add(new InstaUser() { Password = model.Passsword, UserName = model.UserName });
                    await db.SaveChangesAsync();
                    var instaId = db.DbInstaUser.FirstOrDefault(id => id.UserName == model.UserName);
                    db.InstaUserAndAccountUser.Add(new UserInstaAndUser { UserId = user.Id, InstaUserId = instaId.Id });
                    await db.SaveChangesAsync();
                }
                else ModelState.AddModelError("", "Такой аккаунт уже добавлен");
            }
            List<InstaUser> instaUsers = GetInstaUsersForUser();

            //TempData["list"] = instaUsers.ToList();
            return RedirectToAction("AOuth", "Account", new //TODO
            { 
                serializedModel = JsonConvert.SerializeObject(instaUsers.ToList()) 
            });
        }
        public List<InstaUser> GetInstaUsersForUser() // требует исправлении
        {
            //не эфективная выборка 
            List<InstaUser> list = new List<InstaUser>();
            foreach (var item in db.InstaUserAndAccountUser.ToList()) //берем айдишки из базы
            {
                if (item.UserId == user.Id) // 
                {
                    foreach (var instaUs in db.DbInstaUser.ToList())
                    {
                        if (instaUs.Id == item.InstaUserId) { list.Add(instaUs); }
                    }
                }
            }
            return list;
        }
    }
}