using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public class Author:BaseVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MidleName { get; set; }
        [Required]
        public string SecondName { get; set; }

        public ICollection<Book> Books { get; set; }

        public string GetAuthor
        {
            get => SecondName + " , " + FirstName + " " + MidleName;
        }

        public Book Book
        {
            get => default(Book);
            set
            {
            }
        }

        public Book Book1
        {
            get => default(Book);
            set
            {
            }
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
