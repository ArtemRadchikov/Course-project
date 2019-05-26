using DevExpress.Mvvm;
using System.Windows.Input;
using Helper.View;
using Helper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes;
using MaterialDesignThemes.Wpf;

namespace Helper.ViewModel
{
    class MainViewModel:BaseVM
    {
        Page Start;
        Page currentPage;

        
        public ICommand ExetFromApp
        {
            get
            {
                return new DelegateCommand(() =>System.Windows.Application.Current.Shutdown());
            }
        }

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }

        }
        public MainViewModel()
        {
            Start = new StartPage();

            CurrentPage = Start;

            Entrance entrance = new Entrance() { DataContext = new EntranceViewModel() };
            entrance.ShowDialog();
        }

        
    }
}
