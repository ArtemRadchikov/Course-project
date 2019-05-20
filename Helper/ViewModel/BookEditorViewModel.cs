using DevExpress.Mvvm;
using Helper.Model;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public bool HasError { get; set; }

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
                            if(this.Authors.Count == 0 && string.IsNullOrEmpty(this.newAuthor))
                                msg = "Список авторов не может быть пустым \n Учитывайте формат: Фамилия Имя Отчество";
                            else
                                if (!string.IsNullOrEmpty(this.newAuthor) && !isMatch(@"[А-Я][а-я]*\s[А-Я][а-я]*\s[А-Я][а-я]*", this.newAuthor))
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
                            else
                                if (!isMatch(@"[\w,/,-,_,.]+[.][p][d][f]", this.Url))
                                    msg = "Pначение не соответствует формату ссылки, указывающей на pdf файл";
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
                                    msg = "Значение должно целочисленным";
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException(
                        "Unrecognized property: " + columnName);
                }
                if(!string.IsNullOrEmpty(msg))
                    HasError = true;

                return msg;
            }
        }

        public string Description
        {
            get
            {
                HasError = false;
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
                HasError = false;
                OnPropertyChanged("Url");
            }
        }


        public string Publisher {
            get
            {
                HasError = false;
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
                HasError = false;
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
                HasError = false;
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
                HasError = false;
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
                HasError = false;

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
                HasError = false;
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
                HasError = false;
                OnPropertyChanged("KeyWords");
            }
        }

        public ObservableCollection<Author> Authors
        {
            get
            {
                HasError = false;
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
                HasError = false;

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
                HasError = false;
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
                return new DelegateCommand(() => DisposeThis(),()=>!HasError);
            }
        }

        public ICommand Cancel
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedBook = null;
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
