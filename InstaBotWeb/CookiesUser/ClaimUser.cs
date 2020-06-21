using System.Linq;
using System.Security.Claims;

namespace InstaBotWeb.CookiesUser
{
    /// <summary>
    /// Класс для получение данных у Claims
    /// </summary>
    public static class ClaimUser
    {
        /// <summary>
        /// Метод для получение Id пользователя
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static int GetIdUser(ClaimsPrincipal claim)
        {
            if (claim != null)
            {
                var user = claim;
                var claimList = user.Claims.ToList();
                var idUser = claimList[1].Type == "BaseId" ? claimList[1].Value : null;
                return int.Parse(idUser);
            }
            return -1;
        }
    }
}
