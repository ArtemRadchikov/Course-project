using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Helper.ViewModel
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window,IHavePassword
    {
        
        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = new RegistrationViewModel();
        }
        public System.Security.SecureString FPassword
        {
            get
            {
                return FirstPassword.SecurePassword;
            }
        }

        public System.Security.SecureString RPassword
        {
            get
            {
                return PasswordRepit.SecurePassword;
            }
        }
    }

    public interface IHavePassword
    {
        System.Security.SecureString FPassword { get; }
        System.Security.SecureString RPassword { get; }
    }
}
