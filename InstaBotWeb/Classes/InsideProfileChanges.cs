using InstaBotWeb.ViewsModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleAuthorize.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;

namespace InstaBotWeb.Classes
{
    public class InsideProfileChanges : IProfileChanges
    {
        private readonly int idUser;
        private readonly UserProfile profile;
        public InsideProfileChanges(int idUser , UserProfile userProfile)
        {
            this.profile = userProfile;
            this.idUser = idUser;
        }
        public async Task<string> ChangePhoto(IWebHostEnvironment environment)
        {
            if (environment != null)
            {
                bool success = false;
                string uniqueFileName = null;
                string shortPath = null;
                if (profile.Avatar != null)
                {
                    string fileName = profile.Avatar.FileName;
                    string uploadsFolder = $"{environment.WebRootPath}\\img\\photoUsers\\";
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    string photoPath = Path.Combine(uploadsFolder, uniqueFileName);
                    shortPath = "\\img\\photoUsers\\" + uniqueFileName;

                    try
                    {
                        using (var photo = new FileStream(photoPath, FileMode.Create))
                        {
                            await profile.Avatar.CopyToAsync(photo);
                        }
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
               
               
                if (success)
                {
                    return shortPath;
                }
            }
            return "Fail";
        }
        public string ChangePassword(string currentPass)
        {
            if (!string.IsNullOrEmpty(profile.NewPassword) && !string.IsNullOrWhiteSpace(profile.NewPassword))
            {
                IHashMethod hash = new MD5Hash();
                var typeHash = MD5.Create();
                var passHash = hash.GetHashCode(typeHash, profile.NewPassword);
                var lastPassword = hash.GetHashCode(typeHash, profile.CurrentPassword);
                if (lastPassword == currentPass)
                {
                    return passHash;
                }
            }
            return "Fail";
        }
        public string ChangeName()
        {
            if (!string.IsNullOrEmpty(profile.FirstName))
            {
                return profile.FirstName;
            }
            else
            {
                return "Fuck Name";
            }
        }
        public string ChangeLastName()
        {
            if (!string.IsNullOrEmpty(profile.LastName))
            {
                return profile.LastName;
            }
            else
            {
                return "Fuck LastName";
            }
        }
       

    }
}
