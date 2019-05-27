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
using System.Data.Entity;

namespace Helper.ViewModel
{
    class LibraryViewModel:BaseVM
    {
        public ObservableCollection<Book> Books { get; set; }
        public ICollectionView BooksView { get; set; }
        Book selectedBook;
        HelperContext HelperContext = new HelperContext();        

        public string IsAdmin { get;set; }
        
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
                    using (HelperContext helperContext = new HelperContext())
                    {
                        Book bookToRemove = helperContext.Books.Include(b=>b.Authors).Include(b=>b.KeyWords).FirstOrDefault(b => b.BookID == book.BookID);
                        helperContext.Authors.RemoveRange(bookToRemove.Authors);
                        helperContext.KeyWordItems.RemoveRange(bookToRemove.KeyWords);

                        helperContext.Books.Remove(bookToRemove);
                        helperContext.SaveChanges();
                        Books.Remove(book);
                        SelectedBook = null;
                    }

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

                        if (book.Name != "null")
                        {
                            Books.Add(book);
                        }
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
            if(AppUser.GetRoll().Replace("\n","").ToLower() != "admin")
                IsAdmin = null;

            Books = new ObservableCollection<Book>();
            using (HelperContext helperContext = new HelperContext())
            {
                foreach(Book b in helperContext.Books.Include(b=>b.Authors).Include(b => b.KeyWords).ToList())
                {
                    Books.Add(b);
                }
            }
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
