using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class StartPageViewModel:BaseVM
    {
        Page Decisions;
        Page library;
        Page main;
        Page currentPage;
        double frameOpacity;

        public double FrameOpacity
        {
            get => frameOpacity;
            set
            {
                frameOpacity = value;
                OnPropertyChanged("FrameOpacity");
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
        public StartPageViewModel()
        {
            Decisions = new DecisionsPage();
            library = new LibraryPage();
            main = new MainPage() { DataContext = this };
            FrameOpacity = 1;

            CurrentPage = main;
        }

        public ICommand GoToMain
        {
            get
            {
                return new DelegateCommand(() => SlowOpacity(main));
            }
        }

        public ICommand GoToDecisions
        {
            get
            {
                return new DelegateCommand(() => SlowOpacity(Decisions));
            }
        }

        public ICommand GoToLibrary
        {
            get
            {
                return new DelegateCommand(() => SlowOpacity(library));
            }
        }

        async void SlowOpacity(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1.0; i > 0.0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
                CurrentPage = page;
                for (double i = 0.0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(50);
                }
            });
        }
    }
}

