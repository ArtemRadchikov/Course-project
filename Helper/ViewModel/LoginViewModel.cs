using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helper.ViewModel
{
    class LoginViewModel:BaseVM
    {
        private void LoginPassword(SecureString secureString)
        {
            password = ConvertToUnsecureString(secureString);
        }

        string login="";
        string password="";

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public ICommand TryLogin
        {
            get
            {
                return new DelegateCommand<PasswordBox>((parameter) =>
                {
                    try
                    {
                        LoginPassword(parameter.SecurePassword);
                        HelperContext helperContext = new HelperContext();
                        if (helperContext.Users.Any(u => u.Login.Replace("\n", "").Equals(Login) && u.Password.Replace("\n", "").Equals(password)))
                        {
                            User user = helperContext.Users.FirstOrDefault(u => u.Login.Replace("\n", "").Equals(Login) && u.Password.Replace("\n", "").Equals(password));
                            AppUser.SetUser(user);
                            using (StreamWriter sw = new StreamWriter("Roll.txt", false))
                            {
                                sw.WriteLine(user.Rool);
                            }
                                DisposeThis();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль", "Ошибка входа");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);   
                    }

                },!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(password));
            }
        }

        public ICommand CloseApp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    System.Windows.Application.Current.Shutdown();
                });
            }
        }

        public ICommand GoToRegistration
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        RegistrationWindow registrationWindow = new RegistrationWindow();
                        DisposeThis();
                        registrationWindow.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

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