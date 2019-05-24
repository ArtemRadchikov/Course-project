using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public class KeyWordItem : BaseVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KeyWordItemID{get;set;}
        [MaxLength(200)]
        public string Value { get; set; }

        public KeyWordItem(string value)
        {
            Value = value;
        }

        public KeyWordItem() { }

        public ICollection<Book> Books { get; set; }
    }
}
