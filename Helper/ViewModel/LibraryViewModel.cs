using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Helper.Model;
using System.ComponentModel;
using System.Windows.Data;
using DevExpress.Mvvm;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using Helper.View;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Helper.ViewModel
{
    class LibraryViewModel:BaseVM
    {
        public ObservableCollection<Book> Books { get; set; }
        public ICollectionView BooksView { get; set; }
        Book selectedBook;
                
        public Book SelectedBook {
            get => selectedBook; 
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                BooksView.Filter = (obj) =>
                  {
                      if (obj is Book book)
                      {
                          switch (SearchText.FirstOrDefault())
                          {
                              case '@': return book.KeyWords.FirstOrDefault(s => s.Value.ToLower().Contains(SearchText.Remove(0, 1).ToLower())) != null;
                              case '#': return book.Authors.FirstOrDefault(s => s.SecondName.ToLower().Contains(SearchText.Remove(0, 1).ToLower())) != null;
                              case '$': return book.PublishDate == int.Parse(SearchText.Remove(0, 1));

                              default: return book.Name.ToLower().Contains(SearchText.ToLower());
                          }
                      }
                      return false;
                  };
                BooksView.Refresh();
                OnPropertyChanged("SearchText");
            }
        }

        public ICommand Sort
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (BooksView.SortDescriptions.Count > 0)
                    {
                        BooksView.SortDescriptions.Clear();
                    }
                    else
                    {
                        BooksView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    }
                },BooksView.SortDescriptions.Count!=0);
            }
        }

        public ICommand Clear
        {
            get
            {
                return new DelegateCommand(() => SearchText="", ()=>!string.IsNullOrEmpty(SearchText));
            }
        }

        public ICommand DeleteBook
        {
            get
            {
                return new DelegateCommand<Book>((book) =>
                {
                    Books.Remove(book);
                    SelectedBook = Books.FirstOrDefault();

                }, (book) => book != null);
            }
        }
        public ICommand AddItem
        {
            get
            {

                return new DelegateCommand(() =>
                {
                    try
                    {
                        Book book = new Book();
                        var w = new BookEditorWindow();
                        var bm = new BookAddViewModel
                        {
                            SelectedBook = book,
                        };
                        w.DataContext = bm;
                        w.ShowDialog();

                        if (book.Name!= "null")
                            Books.Add(book);
                        BooksView.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            } 
        }

        public ICommand EditBook
        {
            get
            {
                
                return new DelegateCommand<Book>((book) =>
                {
                    try
                    {
                        Book editBook = (Book)this.SelectedBook.Clone();
                        var w = new BookEditorWindow();
                        var bm = new BookAddViewModel
                        {
                            SelectedBook = editBook,
                        };
                        w.DataContext = bm;
                        w.ShowDialog();

                        if (editBook.Name != "null")
                        {
                            Books.Remove(book);
                            Books.Add(editBook);
                            SelectedBook = Books.LastOrDefault();
                            BooksView.Refresh();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }, (book) => book != null);
            }
        }
        
        public LibraryViewModel()
        {
            Books = new ObservableCollection<Book>();

            Book book1 = new Book()
            {
                Name = "Высшая математика",
                Authors = new ObservableCollection<Author>()
                    {
                        new Author(){ FirstName="Жанна",MidleName="Николаевна",SecondName="Горбатович" },
                        new Author(){ FirstName="Александра",MidleName="Сергеевна",SecondName="Семенкова" },
                        new Author(){ FirstName="Елена",MidleName="Алексеевна",SecondName="Шинкевич" }

                },
                KeyWords = new ObservableCollection<KeyWordItem>()
                    {
                        new KeyWordItem("высшая математика"),
                        new KeyWordItem("функции нескольких переменных"),
                        new KeyWordItem("интегралы")
                    },
                PublishDate= 2007,
                Publisher="БГТУ",
                BibliographicDescription= "Высшая математика : учебно-методическое пособие для студентов заочного факультета : в 4 ч. Ч. 3 / [сост.: Ж. Н. Горбатович, А. С. Семенкова, Е. А. Шинкевич] . - Минск : БГТУ , 2007 . - 56 с.",
                Description= "Приведены основные теоретические сведения по изучаемому курсу, примеры решения задач, задачи для самостоятельного решеия по темам \"Функции нескольких переменных\" \"Двойной интеграл\", \"Тройной интеграл\", \"Криволинейные интегралы\"",
                Url= @"https://elib.belstu.by/bitstream/123456789/2998/1/vysshaya-matematika.-ch.3_gorbatovich.pdf"
            };

            Book book2 = new Book()
            {
                Name = "Линейные связности на трехмерных симметрических пространствах",
                Authors = new ObservableCollection<Author>()
                    {
                        new Author(){ FirstName="Наталья",MidleName="Павловна",SecondName="Можей" },
                    },
                KeyWords = new ObservableCollection<KeyWordItem>()
                    {
                        new KeyWordItem("высшая математика"),
                        new KeyWordItem("линейные связности"),
                        new KeyWordItem("трехмерные пространства")
                    },
                PublishDate = 2015,
                Publisher = "БГТУ",
                BibliographicDescription = "Можей, Н. П. Линейные связности на трехмерных симметрических пространствах / Н. П. Можей // Автоматический контроль и автоматизация производственных процессов : материалы Международной научно-технической конференции, 22-24 октября 2015 г. / Белорусский государственный технологический университет ; [редкол.: И. М. Жарский (гл. ред.)]. - Минск : БГТУ, 2015. - С. 185-188.",
                Description = "DescriptionDescriptionDescriptionDescriptionDescription",
                Url = @"https://elib.belstu.by/bitstream/123456789/14752/1/s.-185-188.pdf"
            };

            Books.Add(book2);
            Books.Add(book1);


            BindingOperations.EnableCollectionSynchronization(Books, new object());
            BooksView = CollectionViewSource.GetDefaultView(Books);
        }

        public ICommand KeyWordClick
        {
            get
            {
                return new DelegateCommand<KeyWordItem>((word) =>
                {
                    if (word != null)
                    {
                        SearchText = "@" + word.Value;
                    }
                });
            }
        }

        public ICommand AuthorClick
        {
            get
            {
                return new DelegateCommand<Author>((word) =>
                {
                    if (word != null)
                    {
                        SearchText = "#" + word.SecondName;
                    }
                });
            }
        }

        public ICommand DateClick
        {
            get
            {
                return new DelegateCommand<string>((word) =>
                {
                    SearchText ='$' +word;
                });
            }
        }

        
        public ICommand GoToUrl
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (new Uri(SelectedBook.Url).IsFile)
                    {
                        Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + SelectedBook.Url));
                    }
                    else
                    {
                        Process.Start(SelectedBook.Url);
                    }

                });
            }
        }

    }
}
