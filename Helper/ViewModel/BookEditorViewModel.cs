using DevExpress.Mvvm;
using Helper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper.ViewModel
{
    class BookEditorViewModel : BaseVM
    {
        Book eBook;


        public Book EBook
        {
            get => eBook;
            set
            {
                eBook = value;
                OnPropertyChanged("EBook");
            }
        }

        public ObservableCollection<KeyWordItem> KeyWords
        {
            get => EBook.KeyWords;
            set
            {
                EBook.KeyWords = value;
                OnPropertyChanged("KeyWords");
            }
        }

        string newKeyWord;
        public string NewKeyWord
        {
            get => newKeyWord;
            set
            {
                newKeyWord = value;
                OnPropertyChanged("NewKeyWord");
            }
        }

        public BookEditorViewModel()
        {
            try
            {
                EBook = new Book();
                MessageBox.Show((EBook.KeyWords == null).ToString());
                EBook.KeyWords.Add(new KeyWordItem("www"));
                MessageBox.Show(EBook.KeyWords.Count().ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DelegateCommand AddKeyWord
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    EBook.KeyWords.Add(new KeyWordItem(newKeyWord));
                    MessageBox.Show(EBook.KeyWords[EBook.KeyWords.Count() - 1].Value);

                    newKeyWord = "";
                });
            }
        }

        public DelegateCommand<KeyWordItem> DeleteKeyWord
        {
            get
            {
                return new DelegateCommand<KeyWordItem>((keyword) =>
                {
                    if (keyword != null)
                    {
                        EBook.KeyWords.Remove(keyword);
                    }
                });
            }
        }

    }
}
