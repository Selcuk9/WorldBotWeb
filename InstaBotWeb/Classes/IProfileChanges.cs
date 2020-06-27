using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.Classes
{
    interface IProfileChanges
    {
        public string ChangePassword(string currentPass);
    }
}
