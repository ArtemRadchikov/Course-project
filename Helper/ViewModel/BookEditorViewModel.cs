using DevExpress.Mvvm;
using Helper.Model;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class BookAddViewModel : BaseVM, System.ComponentModel.IDataErrorInfo
    { 
        Book selectedBook;

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = "";
                switch (columnName)
                {
                    case "Publisher":
                        {
                            if (string.IsNullOrEmpty(this.Publisher))
                                msg = "Поле не может быть пустым";
                        }
                        break;
                    case "BookName":
                        {
                            if (string.IsNullOrEmpty(this.BookName))
                                msg = "Поле не может быть пустым";
                        }
                        break;
                    case "NewAuthor":
                        {
                            if(this.Authors.Count == 0 )
                                if(string.IsNullOrEmpty(this.newAuthor))
                                    msg = "Список авторов не может быть пустым \n Учитывайте формат: Фамилия Имя Отчество";
                            else
                                if (!string.IsNullOrEmpty(this.newAuthor) && (!isMatch(@"[А-Я][а-я]*\s[А-Я][а-я]*\s[А-Я][а-я]*", this.newAuthor) || !isMatch(@"[A-Z][a-z]*\s[A-Z][a-z]*\s[A-Z][a-z]*", this.newAuthor)))
                                {
                                    msg = "Введеные данные не соответствуют формату: Фамилия Имя Отчество";
                                }
                        }
                        break;
                    case "NewKeyWord":
                        {
                            if (this.KeyWords.Count == 0)
                                msg = "Список ключевых слов не может быть пустым";
                        }
                        break;
                    case "Description":
                        {
                            if (string.IsNullOrEmpty(this.Description))
                                msg = "Поле не может быть пустым";
                        }
                        break;
                    case "Url":
                        {
                            if (string.IsNullOrEmpty(this.Url))
                                msg = "Поле не может быть пустым";
                            //else
                            //    if (!isMatch(@"[\w,/,-,_,.,:,\s,\d]+[.][p][d][f]", this.Url))
                            //        msg = "Pначение не соответствует формату ссылки, указывающей на pdf файл";
                        }
                        break;
                    case "PublishDate":
                        {
                            if (string.IsNullOrEmpty(this.PublishDate))
                            {
                                msg = "Поле не может быть пустым";
                            }
                            else
                            {
                                if (int.TryParse(this.PublishDate, out int intres))
                                {
                                    if (intres < 1950 || intres > DateTime.Now.Year)
                                        msg = "допустимый диапазон [1950," + DateTime.Now.Year + "]";
                                }
                                else
                                    msg = "Значение должно быть целочисленным";
                            }
                        }
                        break;
                    
                }

                return msg;
            }
        }

        public string Description
        {
            get
            {
                return SelectedBook.Description;
            }
            set
            {
                SelectedBook.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Url
        {
            get => SelectedBook.Url;
            set
            {
                SelectedBook.Url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Publisher {
            get
            {
                return SelectedBook.Publisher;
            }
            set
            {
                SelectedBook.Publisher = value;
                OnPropertyChanged("Publisher");
            }
        }

        public string newAuthor;
        public string NewAuthor
        {
            get
            {
                return newAuthor;
            }
            set
            {
                newAuthor = value;
                OnPropertyChanged("NewAuthor");
            }
        }

        public string newKeyWord;
        public string NewKeyWord
        {
            get
            {
                return newKeyWord;
            }
            set
            {
                newKeyWord = value;
                OnPropertyChanged("NewKeyWord");
            }
        }

        public Book SelectedBook
        {
            get
            {
                return selectedBook;
            }
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        
        public string PublishDate
        {
            get
            {
                if (SelectedBook.PublishDate == 0)
                    return "";
                return SelectedBook.PublishDate.ToString();
            }
            set
            {
                SelectedBook.PublishDate = Convert.ToInt32(value);
                OnPropertyChanged("PublishDate");
            }
        }


        public string BookName
        {
            get
            {
                return SelectedBook.Name;
            }
            set
            {
                SelectedBook.Name=value;
                OnPropertyChanged("BookName");
            }
        }

        public ObservableCollection<KeyWordItem> KeyWords
        {
            get => SelectedBook.KeyWords;
            set
            {
                SelectedBook.KeyWords = value;
                OnPropertyChanged("KeyWords");
            }
        }

        public ObservableCollection<Author> Authors
        {
            get
            {
                return SelectedBook.Authors;
            }
            set
            {
                SelectedBook.Authors = value;
                OnPropertyChanged("Authors");
            }
        }

        public ICommand KeyWordDelete
        {
            get
            {
                return new DelegateCommand<KeyWordItem>((item) =>
                {
                    if (item != null)
                    {
                       KeyWords.Remove(item);
                    }
                });
            }
        }

        public ICommand AuthorDelete
        {
            get
            {
                return new DelegateCommand<Author>((item) =>
                {
                    if (item != null)
                    {
                        Authors.Remove(item);
                    }
                });
            }
        }

        public ICommand AddAuthor
        {
            get
            {
                return new DelegateCommand<string>((author) =>
                {
                    string[] names=author.Split(' ');
                    SelectedBook.Authors.Add(new Author(names[1], names[2], names[0]));
                    NewAuthor = "";
                }, author=>((author.Split(' ')).Length==3) && isMatch(@"[А-Я][а-я]*\s[А-Я][а-я]*\s[А-Я][а-я]*", author));
            }
        }

        bool isMatch(string reg,string observStr)
        {
            Regex regex = new Regex(reg);
            MatchCollection matches = regex.Matches(observStr);
            if (matches.Count == 1)
                return matches[0].Value == observStr;
            else
                return false;

        }

        public ICommand AddKeyWord
        {
            get
            {
                return new DelegateCommand<string>((word) =>
                {
                    SelectedBook.KeyWords.Add(new KeyWordItem(word));
                    NewKeyWord = "";
                },word=>word!="");
            }
        }

        public ICommand Confirm
        {
            get
            {
                return new DelegateCommand(() => 
                {
                    using (HelperContext helperContext = new HelperContext())
                    {
                        using (var transaction = helperContext.Database.BeginTransaction())
                        {
                            try
                            {
                                if (SelectedBook.BookID != 0)
                                {
                                    helperContext.Books.Remove(helperContext.Books.FirstOrDefault(b=>b.BookID==SelectedBook.BookID));
                                    helperContext.SaveChanges();
                                }


                                if (!helperContext.Books.Any(b => b.Name.Replace("\n", "").Equals(SelectedBook.Name) && b.Publisher.Replace("\n", "").Equals(SelectedBook.Publisher) && b.PublishDate.Equals(SelectedBook.PublishDate)))
                                {
                                    helperContext.Books.Add(SelectedBook);
                                    helperContext.SaveChanges();
                                }
                                else
                                    transaction.Rollback();

                                transaction.Commit();
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                transaction.Rollback();
                            }
                        }
                    }

                    DisposeThis();

                },()=>Validate(SelectedBook));
            }
        }

        private static bool Validate(Book book)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(book);
            if (!Validator.TryValidateObject(book, context, results, true))
                return false;
            else
                return true;
        }

        public ICommand Cancel
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedBook.Name="null";
                    DisposeThis();
                });
            }
        }

        void DisposeThis()
        {
            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }

    }
}
