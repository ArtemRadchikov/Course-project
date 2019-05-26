using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    class AppUser
    {
        private static AppUser instance;
        private AppUser(User user) { this.CerrentUser = user; }
        public User CerrentUser { get; private set;}

        public static AppUser getInstance(User user)
        {
            if (instance == null)
                instance = new AppUser(user);
            return instance;
        }
    }
}
