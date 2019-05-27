using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public static class AppUser
    {
        public static User CurrentUser { get;set; }
        
        public static void SetUser(User user)
        {
            CurrentUser = user;
        }
        static AppUser()
        {
            CurrentUser = new User();
        }

        public static string GetRoll()
        {
            string roll;
            using (StreamReader sr = new StreamReader("Roll.txt"))
            {
                roll = sr.ReadLine();
            }
            roll.Replace(@"\n", "");
            return roll;
        }
    }
}
