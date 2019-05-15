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
        public Author(string fn, string mn, string sn)
        {
            FirstName = fn;
            MidleName = mn;
            SecondName = sn;
        }
        public Author() { }
    }
}
