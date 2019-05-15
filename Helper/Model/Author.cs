using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    class Author:BaseVM
    {
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string SecondName { get; set; }
        public string GetAuthor
        {
            get => SecondName + ',' + FirstName + " " + MidleName;
        }
    }
}
