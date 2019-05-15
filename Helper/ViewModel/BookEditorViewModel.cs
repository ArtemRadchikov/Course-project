using DevExpress.Mvvm;
using Helper.Model;
using ReactiveValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class BookAddViewModel : ValidatableObject
    { 
        Book selectedBook;

        private IObjectValidator GetValidator()



        public string newAuthor;
        public string NewAuthor
        {
            get=> newAuthor;
            set
            {
                newAuthor = value;
                OnPropertyChanged("NewAuthor");
            }
        }

        public string newKeyWord;
        public string NewKeyWord
        {
            get => newKeyWord;
            set
            {
                newKeyWord = value;
                OnPropertyChanged("NewKeyWord");
            }
        }

        public Book SelectedBook
        {
            get => selectedBook;
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
                if (int.TryParse(value, out int result))
                {
                    SelectedBook.PublishDate = result;
                    OnPropertyChanged("PublishDate");
                }
                else
                    throw new NotImplementedException();
            }
        }


        public string BookName
        {
            get => SelectedBook.Name;
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
            get => SelectedBook.Authors;
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
                    SelectedBook.Authors.Add(new Author(names[0], names[1], names[2]));
                    NewAuthor = "";
                }, author=>((author.Split(' ')).Length==3) && (author.Split(' '))[2]!="");
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
