using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class RegistrationViewModel: BaseVM,System.ComponentModel.IDataErrorInfo
    {
        string name;
        string login;
        string firstPassword;
        string rPassword;
        bool hasError;
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        bool IsStart=true;

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = "";
                switch (columnName)
                {
                    case "Name":
                        {
                            if (this.Name == "")
                                msg = "Пожалуйста, введите имя.";
                        }
                        break;
                    case "Login":
                        {
                            if (this.Login == "")
                                msg = "Пожалуйста, введите имя.";
                            else
                            {
                                HelperContext helperContext = new HelperContext();
                                if (helperContext.Users.Any(u => u.Login.Replace(@"\n", "").Equals(this.Login)))
                                    msg = "Такой логин занят другим пользователем";
                            }
                        }
                        break;
                }

                if (msg == "")
                    hasError = false;
                else
                    hasError = true;

                return msg;
            }
        }


        private bool EqualPasswords(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString1 = passwordContainer.FPassword;
                var secureString2 = passwordContainer.RPassword;

                firstPassword = ConvertToUnsecureString(secureString1);
                rPassword = ConvertToUnsecureString(secureString2);

                if (firstPassword.Equals(rPassword))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public ICommand RegistrationCommand
        {
            get => new DelegateCommand<object>((param) =>
              {
                  if(!EqualPasswords(param))
                  {
                      MessageBox.Show("Проверьте правильность введнённых паролей.","Пароли не совпадают");
                  }
                  else
                  {
                      if(firstPassword.Length<5)
                          MessageBox.Show("Слишком простой пароль");
                      else
                      {
                          User user = new User() { Name = this.Name, Login = this.Login, Password = this.firstPassword, Rool = "User" };
                          using (HelperContext helperContext = new HelperContext())
                          {
                              helperContext.Users.Add(user);
                              helperContext.SaveChanges();
                              using (StreamWriter sw = new StreamWriter("Roll.txt", false))
                              {
                                  sw.WriteLine("User");
                              }
                          }
                          AppUser.SetUser(user);

                          DisposeThis();
                      }
                  }
              },(param)=> !hasError && !string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.Name));
        }

        public ICommand Back
        {
            get => new DelegateCommand(() =>
            {
                Entrance entrance = new Entrance();
                DisposeThis();
                entrance.ShowDialog();
            });
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


        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
