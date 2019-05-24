using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helper.Model
{
    [BookValidationAttribute]
    public class Book : BaseVM, ICloneable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        [Required]
        public ObservableCollection<KeyWordItem> KeyWords { get; set; } = new ObservableCollection<KeyWordItem>();
        [Required]
        [Range(1950,2019)]
        public int PublishDate { get; set; }
        [Required]
        public string Publisher { get; set; }
        [MaxLength(255)]
        public string BibliographicDescription { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }

        public List<User> Users { get; set; }
        public object Clone()
        {
            ObservableCollection<KeyWordItem> keyWords = new ObservableCollection<KeyWordItem>();
            foreach (var kw in this.KeyWords)
            {
                keyWords.Add(new KeyWordItem(kw.Value));
            }

            ObservableCollection<Author> authors = new ObservableCollection<Author>();
            foreach (var author in this.Authors)
            {
                authors.Add(new Author(author.FirstName, author.MidleName, author.SecondName));
            }

            return new Book()
            {
                BookID=this.BookID,
                KeyWords = keyWords,
                Authors = authors,
                Name = this.Name,
                PublishDate = this.PublishDate,
                Publisher = this.Publisher,
                BibliographicDescription = this.BibliographicDescription,
                Description = this.Description,
                Url = this.Url
            };
        }
    }

    public class BookValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Book book = value as Book;
            if (string.IsNullOrEmpty(book.Publisher))
                return false;
            if (string.IsNullOrEmpty(book.Name))
                return false;
            if(book.Authors.Count == 0)
                return false;
            if (book.KeyWords.Count == 0)
                return false;
            if (string.IsNullOrEmpty(book.Description))
                return false;
            if (string.IsNullOrEmpty(book.Url))
                return false;
            if (book.PublishDate < 1950 || book.PublishDate > DateTime.Now.Year)
                return false;

            //Regex regex = new Regex(@"[\w,/,-,_,.,:,\s\d]+[.][p][d][f]");
            //MatchCollection matches = regex.Matches(book.Url);
            //if (matches.Count == 1)
            //    if( matches[0].Value != book.Url)
            //        return false;

            return true;
        }
    }
}
