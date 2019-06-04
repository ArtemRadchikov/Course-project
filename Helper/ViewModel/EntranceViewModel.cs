using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class EntranceViewModel
    {
        public ICommand Visitor
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    using (StreamWriter sw = new StreamWriter("Roll.txt", false))
                    {
                        sw.WriteLine("Гость");
                    }
                    AppUser.SetUser(new User() { UserID = -1, Name = "Гость", Rool = "Гость" });

                    DisposeThis();
                });
            }
        }

        public ICommand User
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    LoginWindow loginWindow = new LoginWindow();
                    DisposeThis();
                    loginWindow.ShowDialog();
                });
            }
        }

        public ICommand Close
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    System.Windows.Application.Current.Shutdown();
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
